namespace Server.Domain.Entities
{
    public class SkillFeedback
    {
        public SkillFeedback() { }

        private SkillFeedback(
            Guid feedbackId,
            Guid skillId,
            int rating,
            float? assessedExpYears
        )
        {
            FeedbackId = feedbackId;
            SkillId = skillId;
            Rating = rating;
            AssessedExpYears = assessedExpYears;
        }

        public Guid FeedbackId { get; private set; }
        public Guid SkillId { get; private set; }
        public int Rating { get; private set; }
        public float? AssessedExpYears { get; private set; }
        public Feedback Feedback { get; private set; } = null!;
        public Skill Skill { get; private set; } = null!;

        public static SkillFeedback Create(
            Guid feedbackId,
            Guid skillId,
            int rating,
            float? assessedExpYears
        )
        {
            return new SkillFeedback(
                feedbackId,
                skillId,
                rating,
                assessedExpYears
            );
        }

        public void Update(
            int rating,
            float? assessedExpYears
        )
        {
            Rating = rating;
            AssessedExpYears = assessedExpYears;
        }
    }
}
