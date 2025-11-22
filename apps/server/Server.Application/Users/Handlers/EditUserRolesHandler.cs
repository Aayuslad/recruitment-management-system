using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Users.Commands;
using Server.Core.Results;
using Server.Domain.Entities;


namespace Server.Application.Users.Handlers
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
            if (String.IsNullOrEmpty(userIdString))
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: fetch user
            var user = await _userRepository.GetProfileByUserIdAsync(request.UserId, cancellationToken);
            if (user is null)
            {
                return Result.Failure("user not found", 404);
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