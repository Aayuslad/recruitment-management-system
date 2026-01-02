namespace Server.Application.Aggregates.Interviews.Queries.DTOs
{
    public class SkillFeedbackDetailDTO
    {
        public Guid SkillId { get; set; }
        public string SkillName { get; set; } = null!;
        public int Rating { get; set; }
        public float? AssessedExpYears { get; set; }
    }
}