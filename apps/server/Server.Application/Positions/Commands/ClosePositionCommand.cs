using MediatR;

using Server.Core.Results;

namespace Server.Application.Positions.Commands
{
    public class ClosePositionCommand : IRequest<Result>
    {
        public Guid PositionId { get; set; }
        public string ClosureReason { get; set; } = null!;
    }
}