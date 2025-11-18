namespace Server.Application.Interviews.Commands.DTOs
{
    public class InterviewSkillFeedbackDTO
    {
        public Guid SkillId { get; set; }
        public int Rating { get; set; }
        public float? AssessedExpYears { get; set; }
    }
}