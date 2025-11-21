using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Users.Commands;
using Server.Application.Users.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.Users.Handlers
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
                return Result<LoginUserDTO>.Failure("User not found", 404);
            }

            // step 2: verify password
            var passwordVerificationResult = _hasher.Verify(auth.PasswordHash!, request.Password);
            if (!passwordVerificationResult)
            {
                return Result<LoginUserDTO>.Failure("Invalid credentials", 401);
            }

            // step 3: fetch user profile
            var user = await _userRepository.GetProfileByAuthIdAsync(auth.Id, cancellationToken);

            // step 3: generate token
            var token = _jwtTokenGenerator.GenerateToken(auth.Id, user?.Id, auth.UserName);

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