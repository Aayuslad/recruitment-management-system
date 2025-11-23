using MediatR;

using Server.Application.Aggregates.Positions.Queries.DTOs.PositionBatchDTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Positions.Queries
{
    public class GetPositionBatchQuery : IRequest<Result<PositionBatchDetailDTO>>
    {
        public GetPositionBatchQuery(Guid batchId)
        {
            BatchId = batchId;
        }
        public Guid BatchId { get; set; }
    }
}