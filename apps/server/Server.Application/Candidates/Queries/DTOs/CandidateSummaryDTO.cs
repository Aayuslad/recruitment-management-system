using Server.Domain.ValueObjects;

namespace Server.Application.Candidates.Queries.DTOs
{
    public class CandidateSummaryDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string ResumeUrl { get; set; } = null!;
        public bool IsBgVerificationCompleted { get; set; }
    }
}