namespace Server.Domain.Entities
{
    public class EventJobOpening
    {
        private EventJobOpening() { }

        private EventJobOpening(Guid eventId, Guid jobOpeningId)
        {
            EventId = eventId;
            JobOpeningId = jobOpeningId;
        }

        public Guid EventId { get; private set; }
        public Guid JobOpeningId { get; private set; }
        public Event Event { get; private set; } = null!;
        public JobOpening JobOpening { get; private set; } = null!;

        public static EventJobOpening Create(Guid eventId, Guid jobOpeningId)
        {
            return new EventJobOpening(eventId, jobOpeningId);
        }
    }
}