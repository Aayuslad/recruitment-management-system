using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Users.Queries;
using Server.Application.Aggregates.Users.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Users.Handlers
{
    internal class GetUsersSummaryHandler : IRequestHandler<GetUsersSummaryQuery, Result<List<UsersSummaryDetailDTO>>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersSummaryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<List<UsersSummaryDetailDTO>>> Handle(GetUsersSummaryQuery request, CancellationToken cancellationToken)
        {
            // step 1: get all the users
            var users = await _userRepository.GetUsersAsync(cancellationToken);

            // step 2: prepare dto
            var usersDto = new List<UsersSummaryDetailDTO>();
            foreach (var user in users)
            {
                var userDto = new UsersSummaryDetailDTO
                {
                    UserId = user.Id,
                    UserName = user.Auth.UserName,
                    Email = user.Auth.Email.ToString(),
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Status = user.Status,
                    ContactNumber = user.ContactNumber.ToString(),
                    Gender = user.Gender,
                    Roles = user.Roles.Select(
                        selector: x => new UserRolesSummaryDTO
                        {
                            Id = x.RoleId,
                            Name = x.Role.Name
                        }
                    ).ToList(),
                };

                usersDto.Add(userDto);
            }

            // step 3: return result
            return Result<List<UsersSummaryDetailDTO>>.Success(usersDto);
        }
    }
}