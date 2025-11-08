using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Users.Commands;
using Server.Core.Results;

namespace Server.Application.Users.Handlers
{
    internal class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<string>>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserHandler(IAuthRepository authRepository, IUserRepository userRepository, IHasher hasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _hasher = hasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // step 1: check if user exists
            var auth = await _authRepository.GetByUserNameOrEmail(request.UsernameOrEmail, cancellationToken);
            if (auth is null)
            {
                return Result<string>.Failure("User not found", 404);
            }
            var user = await _userRepository.GetByAuthIdAsync(auth.Id);
            if (user is null)
            {
                return Result<string>.Failure("User not found", 404);
            }

            // step 2: verify password
            var passwordVerificationResult = _hasher.Verify(auth.PasswordHash!, request.Password);
            if (!passwordVerificationResult)
            {
                return Result<string>.Failure("Invalid credentials", 401);
            }

            // step 3: generate token
            var token = _jwtTokenGenerator.GenerateToken(auth.Id, user.Id, auth.UserName);

            // step 4: return token
            return Result<string>.Success(token);
        }
    }
}