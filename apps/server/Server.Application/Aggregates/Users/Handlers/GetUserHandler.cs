using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Users.Queries;
using Server.Application.Aggregates.Users.Queries.DTOs;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Users.Handlers
{
    internal class GetUserHandler : IRequestHandler<GetUserQuery, Result<UserDetailDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;

        public GetUserHandler(IUserRepository userRepository, IUserContext userContext)
        {
            _userRepository = userRepository;
            _userContext = userContext;
        }

        public async Task<Result<UserDetailDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch user auth
            var auth = await _userRepository.GetAuthByAuthIdAsync(_userContext.AuthId, cancellationToken);
            if (auth is null)
                throw new NotFoundException("User Not Found.");

            // step 2: fetch user profile
            var user = await _userRepository.GetProfileByAuthIdAsync(_userContext.AuthId, cancellationToken);

            // step 3: prepare DTO
            var userDto = new UserDetailDTO
            {
                IsProfileCompleted = user != null,
                AuthId = auth.Id,
                UserId = user?.Id,
                UserName = auth.UserName,
                Email = auth.Email.ToString(),
                FirstName = user?.FirstName,
                MiddleName = user?.MiddleName,
                LastName = user?.LastName,
                Status = user?.Status,
                ContactNumber = user?.ContactNumber.ToString(),
                IsContactNumberVerified = user?.IsContactNumberVerified,
                Gender = user?.Gender.ToString(),
                Dob = user?.Dob,
                Roles = user?.Roles.Select(
                    selector: x => new UserRolesDTO
                    {
                        Id = x.RoleId,
                        Name = x.Role.Name,
                    }
                ).ToList() ?? [],
            };

            // step 4: return result
            return Result<UserDetailDTO>.Success(userDto);
        }
    }
}