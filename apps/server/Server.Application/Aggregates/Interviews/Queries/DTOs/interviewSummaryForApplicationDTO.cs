using Server.Domain.Enums;

namespace Server.Application.Aggregates.Interviews.Queries.DTOs
{
    public class InterviewSummaryForApplicationDTO
    {
        public Guid Id { get; set; }
        public int RoundNumber { get; set; }
        public InterviewType InterviewType { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public int DurationInMinutes { get; set; }
        public InterviewStatus Status { get; set; }
        public ICollection<InterviewParticipantDetailDTO> Participants { get; set; } =
            new List<InterviewParticipantDetailDTO>();
    }
}