using MediatR;

using Server.Application.Aggregates.Users.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Users.Queries
{
    public class GetUserQuery : IRequest<Result<UserDetailDTO>>
    {
        public GetUserQuery(Guid authId)
        {
            AuthId = authId;
        }

        public Guid AuthId { get; set; }
    }
}