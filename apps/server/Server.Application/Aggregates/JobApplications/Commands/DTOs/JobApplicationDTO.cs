namespace Server.Application.Aggregates.JobApplications.Commands.DTOs
{
    public class JobApplicationDTO
    {
        public Guid CandidateId { get; set; }
        public Guid JobOpeningId { get; set; }
    }
}