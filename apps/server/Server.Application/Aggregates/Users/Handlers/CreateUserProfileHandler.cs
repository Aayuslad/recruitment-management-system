using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Users.Commands;
using Server.Application.Aggregates.Users.Commands.DTOs;
using Server.Application.Exeptions;
using Server.Core.Results;
using Server.Domain.Entities.Users;
using Server.Domain.ValueObjects;

namespace Server.Application.Aggregates.Users.Handlers
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
                throw new UnAuthorisedExeption();
            }

            // step 1: check if profile is already there for the auth
            var result = await _userRepository.ProfileExistsByAuthIdAsync(Guid.Parse(authId), cancellationToken);
            if (result == true)
            {
                throw new ConflictExeption("User profile already exists.");
            }

            // step 2: create all VOs
            var contactNumber = ContactNumber.Create(request.ContactNumber);

            // step 3: check if user with same contact number exists
            if (await _userRepository.ProfileExistsByContactNumberAsync(contactNumber, cancellationToken))
            {
                throw new ConflictExeption("User with same contact number already exists.");
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