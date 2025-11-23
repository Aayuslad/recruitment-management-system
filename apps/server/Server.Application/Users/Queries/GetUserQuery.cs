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

        //TODO: extact from token and remove from here
        public Guid AuthId { get; set; }
    }
}