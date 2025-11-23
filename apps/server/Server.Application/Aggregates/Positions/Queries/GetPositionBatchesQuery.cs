using MediatR;

using Server.Application.Aggregates.Positions.Queries.DTOs.PositionBatchDTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Positions.Queries
{
    public class GetPositionBatchesQuery : IRequest<Result<List<PositionBatchSummaryDTO>>>
    {
        // add pagination later
    }
}