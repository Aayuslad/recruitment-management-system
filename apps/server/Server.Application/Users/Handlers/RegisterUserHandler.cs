using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Exeptions;
using Server.Application.Users.Commands;
using Server.Application.Users.Commands.DTOs;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.ValueObjects;

namespace Server.Application.Users.Handlers
{
    internal sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RegisterUserHandler(IHasher hasher, IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _hasher = hasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<Result<RegisterUserDTO>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // step 1: check VOs 
            var email = Email.Create(request.Email);

            // step 1: check if user auth already exists
            if (await _userRepository.AuthExistsByUserNameAsync(request.UserName, cancellationToken))
            {
                throw new ConflictExeption("User with this username already exists.");
            }
            if (await _userRepository.AuthExistsByEmailAsync(email, cancellationToken))
            {
                throw new ConflictExeption("User with this email already exists.");
            }

            // step 2: hash password
            var hashedPassword = _hasher.Hash(request.Password);

            // step 3: create user auth
            var auth = Auth.Create(request.UserName, email, hashedPassword, null);

            // step 4: persist user auth
            await _userRepository.AddAuthAsync(auth, cancellationToken);

            // step 5: generate JWT token
            var token = _jwtTokenGenerator.GenerateToken(auth.Id, null, auth.UserName);

            // step 6: return result
            var registerUserDto = new RegisterUserDTO
            {
                Token = token
            };
            return Result<RegisterUserDTO>.Success(registerUserDto);
        }
    }
}