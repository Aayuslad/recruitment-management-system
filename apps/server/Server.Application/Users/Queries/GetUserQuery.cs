using MediatR;

using Server.Application.DTOs;
using Server.Core.Results;

namespace Server.Application.Users.Queries
{
    public class GetUserQuery : IRequest<Result<UserDTO>>
    {
        public GetUserQuery(Guid authId)
        {
            AuthId = authId;
        }

        public Guid AuthId { get; set; }
    }
}