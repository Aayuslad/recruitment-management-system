namespace Server.Application.JobApplications.Queries.DTOs
{
    public class SkillFeedbackDetailDTO
    {
        public Guid SkillId { get; set; }
        public string SkillName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public float? AssessedExpYears { get; set; }
    }
}
