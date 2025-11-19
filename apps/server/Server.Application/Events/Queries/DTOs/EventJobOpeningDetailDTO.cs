namespace Server.Application.Events.Queries.DTOs
{
    public class EventJobOpeningDetailDTO
    {
        public Guid JobOpeningId { get; set; }
        public string JobOpeningTitle { get; set; } = null!;
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; } = null!;
    }
}