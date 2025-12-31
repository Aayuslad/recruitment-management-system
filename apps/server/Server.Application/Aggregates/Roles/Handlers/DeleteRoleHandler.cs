using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Roles.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Roles.Handlers
{
    internal class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, Result>
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IUserContext _userContext;

        public DeleteRoleHandler(IRolesRepository rolesRepository, IUserContext userContext)
        {
            _rolesRepository = rolesRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch the entity
            var role = await _rolesRepository.GetByIdAsync(request.Id, cancellationToken);
            if (role is null)
            {
                throw new NotFoundException("Role Not Found.");
            }

            // step 2: soft delete
            role.Delete(_userContext.UserId);

            // step 3: persist
            await _rolesRepository.UpdateAsync(role, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}