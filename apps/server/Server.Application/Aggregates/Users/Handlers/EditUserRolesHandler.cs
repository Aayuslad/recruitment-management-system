using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Users.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities.Users;


namespace Server.Application.Aggregates.Users.Handlers
{
    internal class EditUserRolesHandler : IRequestHandler<EditUserRolesCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;

        public EditUserRolesHandler(IUserRepository userRepository, IUserContext userContext)
        {
            _userRepository = userRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(EditUserRolesCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch user
            var user = await _userRepository.GetProfileByUserIdAsync(request.UserId, cancellationToken);
            if (user is null)
            {
                throw new NotFoundException("User Not Found.");
            }

            // step 2: check if admin is trying to remove admin role
            var isEditingAdminUser = user.Roles.Any(x => x.Role.Name == "Admin");
            if (isEditingAdminUser)
            {
                var adminRoleId = user.Roles.First(x => x.Role.Name == "Admin").RoleId;
                if (!request.Roles.Any(x => x.RoleId == adminRoleId))
                {
                    throw new ConflictException("You cannot remove admin role.");
                }
            }

            // step 2: edit roles
            user.SyncRoles(
                newRoles: request.Roles.Select(
                    selector: x => UserRole.Create(
                        userId: user.Id,
                        roleId: x.RoleId,
                        assignedBy: x.AssignedBy ?? _userContext.UserId
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