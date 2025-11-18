using Server.Domain.Enums;

namespace Server.Application.Interviews.Queries.DTOs
{
    public class InterviewDetailDTO
    {
        public Guid Id { get; set; }
        public Guid JobApplicationId { get; set; }
        public int RoundNumber { get; set; }
        public InterviewType InterviewType { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        public string? MeetingLink { get; set; }
        public InterviewStatus Status { get; set; }
        public List<FeedbackDetailDTO> Feedbacks { get; set; } =
            new List<FeedbackDetailDTO>();
        public List<InterviewParticipantDetailDTO> Participants { get; set; } =
            new List<InterviewParticipantDetailDTO>();
    }
}