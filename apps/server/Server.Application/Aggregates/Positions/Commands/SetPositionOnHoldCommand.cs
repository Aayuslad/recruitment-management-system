using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Positions.Commands
{
    public class SetPositionOnHoldCommand : IRequest<Result>
    {
        public Guid PositionId { get; set; }
        public string? Comments { get; set; }
    }
}