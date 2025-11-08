using MediatR;

using Server.Application.Positions.Queries.DTOs.PositionDTOs;
using Server.Core.Results;

namespace Server.Application.Positions.Queries
{
    public class GetPositionsQuery : IRequest<Result<List<PositionSummaryDTO>>>
    {
    }
}