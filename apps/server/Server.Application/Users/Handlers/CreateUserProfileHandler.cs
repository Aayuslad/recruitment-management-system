using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Users.Commands;
using Server.Application.Users.Commands.DTOs;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.ValueObjects;

namespace Server.Application.Users.Handlers
{
    internal class CreateUserprofileHandler : IRequestHandler<CreateUserProfileCommand, Result<CreateUserProfileDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public CreateUserprofileHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<CreateUserProfileDTO>> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var authId = _httpContextAccessor.HttpContext?.User.FindFirst("authId")?.Value;
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst("userName")?.Value;
            if (authId is null)
            {
                return Result<CreateUserProfileDTO>.Failure("Unauthorised", 401);
            }

            // step 1: check if profile is already there for the auth
            var result = await _userRepository.ProfileExistsByAuthIdAsync(Guid.Parse(authId), cancellationToken);
            if (result == true)
            {
                return Result<CreateUserProfileDTO>.Failure("A profile already exists for this user.", 409);
            }

            // step 2: create all VOs
            var contactNumResult = ContactNumber.Create(request.ContactNumber);
            if (contactNumResult.IsSuccess == false)
            {
                return Result<CreateUserProfileDTO>.Failure(contactNumResult.ErrorMessage ?? "Invalid contact number", contactNumResult.StatusCode);
            }
            var contactNumber = contactNumResult.Value!;

            // step 3: check if user with same contact number exists
            if (await _userRepository.ProfileExistsByContactNumberAsync(contactNumber, cancellationToken))
            {
                return Result<CreateUserProfileDTO>.Failure("User with the same contact number already exists.", 409);
            }

            // step 4: create user entity
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

            // step 5: save user profile
            await _userRepository.AddProfileAsync(user, cancellationToken);

            // step 6: create jwt token
            var token = _jwtTokenGenerator.GenerateToken(Guid.Parse(authId), user.Id, userName ?? "");

            // step 7: return result
            var createUserProfileDTO = new CreateUserProfileDTO
            {
                Token = token
            };
            return Result<CreateUserProfileDTO>.Success(createUserProfileDTO);
        }
    }
}