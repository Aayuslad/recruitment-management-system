using Server.Domain.Enums;

namespace Server.Application.JobApplications.Queries.DTOs
{
    public class JobApplicationDetailDTO
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }
        public string CandidateName { get; set; } = null!;
        public Guid JobOpeningId { get; set; }
        public DateTime AppliedAt { get; set; }
        public JobApplicationStatus Status { get; set; }
        public string Designation { get; set; } = null!;
        public double? AvgRating { get; set; }
        public List<FeedbackDetailDTO> Feedbacks { get; set; } =
            new List<FeedbackDetailDTO>();
        public List<StatusMoveHistoryDetailDTO> StatusMoveHistories { get; set; } =
            new List<StatusMoveHistoryDetailDTO>();
    }
}