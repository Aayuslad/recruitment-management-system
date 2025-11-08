using MediatR;

using Server.Application.Positions.Queries.DTOs.PositionBatchDTOs;
using Server.Core.Results;

namespace Server.Application.Positions.Queries
{
    public class GetPositionBatchesQuery : IRequest<Result<List<PositionBatchSummaryDTO>>>
    {
        // add pagination later
    }
}