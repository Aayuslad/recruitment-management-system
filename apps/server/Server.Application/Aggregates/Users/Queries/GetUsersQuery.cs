using MediatR;

using Server.Application.Aggregates.Users.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Users.Queries
{
    public class GetUsersQuery : IRequest<Result<List<UsersDetailDTO>>>
    {
    }
}