using MediatR;

using Server.Core.Results;

namespace Server.Application.Positions.Commands
{
    public class SetPositionOnHoldCommand : IRequest<Result>
    {
        public Guid PositionId { get; set; }
        public string? comments { get; set; }
    }
}