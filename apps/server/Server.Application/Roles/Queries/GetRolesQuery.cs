using MediatR;

using Server.Application.Roles.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Roles.Queries
{
    public class GetRolesQuery : IRequest<Result<List<RoleDetailDTO>>>
    {
    }
}