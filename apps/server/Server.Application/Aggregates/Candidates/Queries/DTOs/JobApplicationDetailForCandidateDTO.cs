using Server.Domain.Enums;

namespace Server.Application.Aggregates.Candidates.Queries.DTOs
{
    public class JobApplicationDetailForCandidateDTO
    {
        public Guid Id { get; set; }
        public string DesignationName { get; set; } = null!;
        public DateTime AppliedAt { get; set; }
        public string JobLocation { get; set; } = null!;
        public JobApplicationStatus Status { get; set; }
    }
}