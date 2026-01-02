using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobApplications.Queries.DTOs
{
    public class JobOpeningApplicationSummaryDTO
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }
        public string CandidateName { get; set; } = null!;
        public DateTime AppliedAt { get; set; }
        public JobApplicationStatus Status { get; set; }
    }
}