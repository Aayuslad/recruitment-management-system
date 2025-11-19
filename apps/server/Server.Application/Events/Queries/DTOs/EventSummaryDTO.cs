using Server.Domain.Enums;

namespace Server.Application.Events.Queries.DTOs
{
    public class EventSummaryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public EventType Type { get; set; }
        public ICollection<EventJobOpeningDetailDTO> JobOpenings { get; set; } =
            new List<EventJobOpeningDetailDTO>();
    }
}