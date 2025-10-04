using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Users.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.ValueObjects;

namespace Server.Application.Users.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateUserHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var authId = _httpContextAccessor.HttpContext?.User.FindFirst("authId")?.Value;
            if (authId == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            var result = await _userRepository.ExistsByAuthId(Guid.Parse(authId), cancellationToken);
            if (result == true)
            {
                return Result.Failure("A profile already exists for this user.", 409);
            }

            var contactNumResult = ContactNumber.Create(request.ContactNumber);
            if (contactNumResult.IsSuccess == false)
            {
                return Result.Failure(contactNumResult.ErrorMessage ?? "Invalid contact number", contactNumResult.StatusCode);
            }
            var contactNumber = contactNumResult.Value!;

            // step 1: check if auth with same contact number exists
            if (await _userRepository.ExistsByContactNumberAsync(contactNumber, cancellationToken))
            {
                return Result.Failure("User with the same contact number already exists.", 409);
            }

            // step 2: create user entity
            var user = User.Create(
                Guid.Parse(authId),
                request.FirstName,
                request.MiddleName,
                request.LastName,
                null,
                contactNumber,
                request.Gender,
                request.Dob
            );

            // step 3: save user entity
            await _userRepository.AddAsync(user, cancellationToken);

            // step 4: return success result
            return Result.Success();
        }
    }
}