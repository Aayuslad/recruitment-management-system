using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Users.Queries;
using Server.Application.Aggregates.Users.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Users.Handlers
{
    internal class GetUsersHandler : IRequestHandler<GetUsersQuery, Result<List<UsersDetailDTO>>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<List<UsersDetailDTO>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            // step 1: get all the users
            var users = await _userRepository.GetUsersAsync(cancellationToken);

            // step 2: prepare dto
            var usersDto = new List<UsersDetailDTO>();
            foreach (var user in users)
            {
                var userDto = new UsersDetailDTO
                {
                    UserId = user.Id,
                    UserName = user.Auth.UserName,
                    Email = user.Auth.Email.ToString(),
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Status = user.Status,
                    ContactNumber = user.ContactNumber.ToString(),
                    IsContactNumberVerified = user.IsContactNumberVerified,
                    Gender = user.Gender,
                    Dob = user.Dob,
                    Roles = user.Roles.Select(x => new UserRolesDTO
                    {
                        Id = x.RoleId,
                        Name = x.Role.Name,
                        AssignedBy = x.AssignedBy,
                    }
                    ).ToList(),
                };

                usersDto.Add(userDto);
            }

            // step 3: return result
            return Result<List<UsersDetailDTO>>.Success(usersDto);
        }
    }
}