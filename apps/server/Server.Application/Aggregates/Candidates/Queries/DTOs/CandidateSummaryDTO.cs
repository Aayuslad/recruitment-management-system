using Server.Domain.Enums;

namespace Server.Application.Aggregates.Candidates.Queries.DTOs
{
    public class CandidateSummaryDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public Gender Gender { get; set; }
        public string ContactNumber { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string CollegeName { get; set; } = null!;
        public string ResumeUrl { get; set; } = null!;
        public bool IsBgVerificationCompleted { get; set; }
        public List<JobApplicationSummaryForCandidateDTO> JobApplications { get; set; } =
            new List<JobApplicationSummaryForCandidateDTO>();
        public DateTime CreatedAt { get; set; }
    }
}