using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Users.Commands;
using Server.Application.Exeptions;
using Server.Core.Results;
using Server.Domain.Entities.Users;


namespace Server.Application.Aggregates.Users.Handlers
{
    internal class EditUserRolesHandler : IRequestHandler<EditUserRolesCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditUserRolesHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(EditUserRolesCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UnAuthorisedExeption();
            }

            // step 1: fetch user
            var user = await _userRepository.GetProfileByUserIdAsync(request.UserId, cancellationToken);
            if (user is null)
            {
                throw new NotFoundExeption("User Not Found.");
            }

            // step 2: check if admin is trying to remove admin role
            var isEditingAdminUser = user.Roles.Any(x => x.Role.Name == "Admin");
            if (isEditingAdminUser)
            {
                var adminRoleId = user.Roles.First(x => x.Role.Name == "Admin").RoleId;
                if (!request.Roles.Any(x => x.RoleId == adminRoleId))
                {
                    throw new ConflictExeption("You cannot remove admin role.");
                }
            }

            // step 2: edit roles
            user.SyncRoles(
                newRoles: request.Roles.Select(
                    selector: x => UserRole.Create(
                        userId: user.Id,
                        roleId: x.RoleId,
                        assignedBy: x.AssignedBy ?? Guid.Parse(userIdString)
                    )
                ).ToList()
            );

            // step 3: persist entity
            await _userRepository.UpdateProfileAsync(user, cancellationToken);

            // step 4: retuen result
            return Result.Success();
        }
    }
}