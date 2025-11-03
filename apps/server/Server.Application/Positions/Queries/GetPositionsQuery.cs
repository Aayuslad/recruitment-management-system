using MediatR;

using Server.Application.Positions.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Positions.Queries
{
    public class GetPositionsQuery : IRequest<Result<PositionsDetailDTO>>
    {
    }
}