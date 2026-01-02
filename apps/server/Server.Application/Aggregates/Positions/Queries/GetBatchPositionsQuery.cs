using MediatR;

using Server.Application.Aggregates.Positions.Queries.DTOs.PositionDTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Positions.Queries
{
    public class GetBatchPositionsQuery : IRequest<Result<List<BatchPositionSummaryDTO>>>
    {
        public GetBatchPositionsQuery(Guid id)
        {
            BatchId = id;
        }

        public Guid BatchId { get; set; }
    }
}