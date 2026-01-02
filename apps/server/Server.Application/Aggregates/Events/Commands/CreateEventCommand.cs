using MediatR;

using Server.Application.Aggregates.Events.Commands.DTOs;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.Events.Commands
{
    public class CreateEventCommand : IRequest<Result>
    {
        public string Name { get; set; } = null!;
        public EventType Type { get; set; }
        public ICollection<EventJobOpeningDTO> JobOpenings { get; set; } =
            new List<EventJobOpeningDTO>();
    }
}