using Server.Domain.Enums;

namespace Server.Application.Aggregates.Interviews.Queries.DTOs
{
    public class InterviewSummaryDTO
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }
        public string CandidateName { get; set; } = null!;
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; } = null!;
        public int RoundNumber { get; set; }
        public InterviewType InterviewType { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public int DurationInMinutes { get; set; }
        public InterviewStatus Status { get; set; }
    }
}