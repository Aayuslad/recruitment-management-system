
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Roles.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities.Roles;

namespace Server.Application.Aggregates.Roles.Handlers
{
    internal class CreateRoleHandler : IRequestHandler<CreateRoleCommand, Result>
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateRoleHandler(IRolesRepository rolesRepository, IHttpContextAccessor httpContextAccessor)
        {
            _rolesRepository = rolesRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedException();
            }

            // step 1: check if with this name a role exsist
            var result = await _rolesRepository.ExistsByNameAsync(request.Name, cancellationToken);
            if (result)
            {
                throw new ConflictException($"Role with name {request.Name} already exsist");
            }

            // step 2: make the entity
            var role = Role.Create(
                name: request.Name,
                description: request.Description,
                createdBy: Guid.Parse(userIdString)
            );

            // step 3: persist entity
            await _rolesRepository.AddAsync(role, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}