using MediatR;

using Server.Application.Users.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Users.Queries
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