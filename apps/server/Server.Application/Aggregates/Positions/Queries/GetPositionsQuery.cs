using MediatR;

using Server.Application.Aggregates.Positions.Queries.DTOs.PositionDTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Positions.Queries
{
    public class GetPositionsQuery : IRequest<Result<List<PositionSummaryDTO>>>
    {
    }
}