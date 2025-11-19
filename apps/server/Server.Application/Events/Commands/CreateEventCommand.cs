using MediatR;

using Server.Application.Events.Commands.DTOs;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Events.Commands
{
    public class CreateEventCommand : IRequest<Result>
    {
        public string Name { get; set; } = null!;
        public EventType Type { get; set; }
        public ICollection<EventJobOpeningDTO> JobOpenings { get; set; } =
            new List<EventJobOpeningDTO>();
    }
}