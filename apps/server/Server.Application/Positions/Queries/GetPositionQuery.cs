using MediatR;

using Server.Application.Positions.Queries.DTOs.PositionDTOs;
using Server.Core.Results;

namespace Server.Application.Positions.Queries
{
    public class GetPositionQuery : IRequest<Result<PositionDetailDTO>>
    {
        public GetPositionQuery(Guid id)
        {
            PositionId = id;
        }

        public Guid PositionId { get; set; }
    }
}