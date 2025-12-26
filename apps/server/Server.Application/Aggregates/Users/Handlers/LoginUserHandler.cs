using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Users.Commands;
using Server.Application.Aggregates.Users.Commands.DTOs;
using Server.Application.Exeptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Users.Handlers
{
    internal class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<LoginUserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserHandler(IUserRepository userRepository, IHasher hasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<LoginUserDTO>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // step 1: check if user auth exists
            var auth = await _userRepository.GetAuthByEmailOrUserNameAsync(request.UsernameOrEmail, cancellationToken);
            if (auth is null)
            {
                throw new NotFoundExeption("User not found");
            }

            // step 2: verify password
            var passwordVerificationResult = _hasher.Verify(auth.PasswordHash!, request.Password);
            if (!passwordVerificationResult)
            {
                throw new UnAuthorisedExeption("Invalid credentials");
            }

            // step 3: fetch user profile
            var user = await _userRepository.GetProfileByAuthIdAsync(auth.Id, cancellationToken);

            // step 3: generate token
            var token = _jwtTokenGenerator.GenerateToken(auth.Id, user?.Id, auth.UserName, user?.Roles.Select(x => x.Role.Name).ToList());

            // step 4: return token
            var loginUserDto = new LoginUserDTO
            {
                Token = token,
                IsProfileCompleted = user != null,
            };
            return Result<LoginUserDTO>.Success(loginUserDto);
        }
    }
}