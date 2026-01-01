using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Users.Commands;
using Server.Application.Aggregates.Users.Commands.DTOs;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities.Users;
using Server.Domain.ValueObjects;

namespace Server.Application.Aggregates.Users.Handlers
{
    internal class CreateUserprofileHandler : IRequestHandler<CreateUserProfileCommand, Result<CreateUserProfileDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public CreateUserprofileHandler(IUserRepository userRepository, IUserContext userContext, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _userContext = userContext;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<CreateUserProfileDTO>> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var authId = _userContext.AuthId;

            // step 1: check if the auth exists
            var auth = await _userRepository.GetAuthByAuthIdAsync(authId, cancellationToken);
            if (auth is null)
            {
                throw new UnAuthorisedException();
            }

            // step 2: check if profile is already there for the auth
            var result = await _userRepository.ProfileExistsByAuthIdAsync(authId, cancellationToken);
            if (result == true)
            {
                throw new ConflictException("User profile already exists.");
            }

            // step 3: create all VOs
            var contactNumber = ContactNumber.Create(request.ContactNumber);

            // step 4: check if user with same contact number exists
            if (await _userRepository.ProfileExistsByContactNumberAsync(contactNumber, cancellationToken))
            {
                throw new ConflictException("User with same contact number already exists.");
            }

            // step 5: create user entity
            var user = User.Create(
                authId,
                request.FirstName,
                request.MiddleName,
                request.LastName,
                null,
                contactNumber,
                request.Gender,
                request.Dob
            );

            // step 6: save user profile
            await _userRepository.AddProfileAsync(user, cancellationToken);

            // step 7: create jwt token
            var token = _jwtTokenGenerator.GenerateToken(authId, user.Id, auth.UserName ?? "");

            // step 8: return result
            var createUserProfileDTO = new CreateUserProfileDTO
            {
                Token = token
            };
            return Result<CreateUserProfileDTO>.Success(createUserProfileDTO);
        }
    }
}