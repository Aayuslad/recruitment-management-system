
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Roles.Commands;
using Server.Core.Results;

namespace Server.Application.Roles.Handlers
{
    internal class EditRoleHandler : IRequestHandler<EditRoleCommand, Result>
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditRoleHandler(IRolesRepository rolesRepository, IHttpContextAccessor httpContextAccessor)
        {
            _rolesRepository = rolesRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: fetch the role
            var role = await _rolesRepository.GetByIdAsync(request.Id, cancellationToken);
            if (role is null)
            {
                return Result.Failure("Role does not exist", 404);
            }

            // step 2: update
            role.Update(
                name: request.Name,
                description: request.Description,
                updatedBy: Guid.Parse(userIdString)
            );

            // step 3: persist entity
            await _rolesRepository.UpdateAsync(role, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}