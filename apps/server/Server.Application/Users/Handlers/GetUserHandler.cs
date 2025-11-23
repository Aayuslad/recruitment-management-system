using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Exeptions;
using Server.Application.Users.Queries;
using Server.Application.Users.Queries.DTOs;
using Server.Core.Results;

namespace Server.API.Controllers
{
    internal class GetUserHandler : IRequestHandler<GetUserQuery, Result<UserDetailDTO>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserDetailDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch user auth
            var auth = await _userRepository.GetAuthByAuthIdAsync(request.AuthId, cancellationToken);
            if (auth is null)
                throw new NotFoundExeption("User Not Found.");

            // step 2: fetch user profile
            var user = await _userRepository.GetProfileByAuthIdAsync(request.AuthId, cancellationToken);

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