using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Events.Commands
{
    public class DeleteEventCommand : IRequest<Result>
    {
        public DeleteEventCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}