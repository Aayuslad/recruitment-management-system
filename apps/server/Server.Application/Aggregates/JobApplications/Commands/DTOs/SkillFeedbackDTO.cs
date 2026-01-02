namespace Server.Application.Aggregates.JobApplications.Commands.DTOs
{
    public class SkillFeedbackDTO
    {
        public Guid SkillId { get; set; }
        public int Rating { get; set; }
        public float? AssessedExpYears { get; set; }
    }
}