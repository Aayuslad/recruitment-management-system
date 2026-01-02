using MediatR;

using Server.Application.Aggregates.Roles.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Roles.Queries
{
    public class GetRolesQuery : IRequest<Result<List<RoleDetailDTO>>>
    {
    }
}