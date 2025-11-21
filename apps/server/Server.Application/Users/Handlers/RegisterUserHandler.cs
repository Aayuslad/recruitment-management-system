using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
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
            // TODO: this can be moved in VO by throwing exeption
            var emailResult = Email.Create(request.Email);
            if (emailResult.IsSuccess == false)
            {
                return Result<RegisterUserDTO>.Failure(emailResult.ErrorMessage ?? "Invalid Email", emailResult.StatusCode);
            }
            var email = emailResult.Value!;

            // step 1: check if user auth already exists
            if (await _userRepository.AuthExistsByUserNameAsync(request.UserName, cancellationToken))
            {
                return Result<RegisterUserDTO>.Failure("Username is already taken.", 409);
            }
            if (await _userRepository.AuthExistsByEmailAsync(email, cancellationToken))
            {
                return Result<RegisterUserDTO>.Failure("User with this email already exists.", 409);
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