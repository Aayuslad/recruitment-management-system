using MediatR;

using Microsoft.Extensions.Logging;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Users.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.ValueObjects;

namespace Server.Application.Users.Handlers
{
    internal sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<string>>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IHasher _hasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(IAuthRepository authRepository, IHasher hasher, ILogger<RegisterUserHandler> logger, IJwtTokenGenerator jwtTokenGenerator)
        {
            _authRepository = authRepository;
            _hasher = hasher;
            _logger = logger;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var emailResult = Email.Create(request.Email);
            if (emailResult.IsSuccess == false)
            {
                return Result<string>.Failure(emailResult.ErrorMessage ?? "Invalid Email", emailResult.StatusCode);
            }
            var email = emailResult.Value!;

            // step 1: check if user already exists
            if (await _authRepository.ExistsByUserNameAsync(request.UserName, cancellationToken))
            {
                return Result<string>.Failure("Username is already taken.", 409);
            }
            if (await _authRepository.ExistsByEmailAsync(email, cancellationToken))
            {
                return Result<string>.Failure("User with this email already exists.", 409);
            }

            // step 2: hash password
            var hashedPassword = _hasher.Hash(request.Password);

            // step 3: create user
            var auth = Auth.Create(request.UserName, email, hashedPassword, null);

            // step 4: persist user
            await _authRepository.AddAsync(auth, cancellationToken);
            _logger.LogInformation("New user registered: {Username}, {Email}", request.UserName, request.Email);

            // step 5: generate JWT token
            var token = _jwtTokenGenerator.GenerateToken(auth.Id, null, auth.UserName);

            // step 6: return result
            return Result<string>.Success(token);
        }
    }
}