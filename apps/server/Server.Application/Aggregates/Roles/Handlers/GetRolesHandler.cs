using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Roles.Queries;
using Server.Application.Aggregates.Roles.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Roles.Handlers
{
    internal class GetRolesHandler : IRequestHandler<GetRolesQuery, Result<List<RoleDetailDTO>>>
    {
        private readonly IRolesRepository _rolesRepository;

        public GetRolesHandler(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public async Task<Result<List<RoleDetailDTO>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch roles
            var roles = await _rolesRepository.GetAllAsync(cancellationToken);

            // step 2: make dto list
            var roleDtos = roles.Select(
                selector: x => new RoleDetailDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                }
            ).ToList();

            // step 3: return result
            return Result<List<RoleDetailDTO>>.Success(roleDtos);
        }
    }
}