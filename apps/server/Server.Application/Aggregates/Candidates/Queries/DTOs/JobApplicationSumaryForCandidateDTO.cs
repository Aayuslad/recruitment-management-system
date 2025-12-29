using Server.Domain.Enums;

namespace Server.Application.Aggregates.Candidates.Queries.DTOs
{
    public class JobApplicationSummaryForCandidateDTO
    {
        public JobApplicationStatus Status { get; set; }
    }
}