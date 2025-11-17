using Server.Core.Entities;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class Feedback : BaseEntity<Guid>
    {
        private Feedback() : base(Guid.Empty) { }

        private Feedback(
            Guid? id,
            Guid? jobApplicationId,
            Guid givenById,
            FeedbackStage stage,
            string? comment,
            int rating,
            IEnumerable<SkillFeedback> skillFeedbacks
        ) : base(id ?? Guid.NewGuid())
        {
            JobApplicationId = jobApplicationId;
            GivenById = givenById;
            Stage = stage;
            Comment = comment;
            Rating = rating;

            SkillFeedbacks = skillFeedbacks.ToList();
        }

        public Guid? JobApplicationId { get; private set; }
        // TODO: add when interview entity is created
        //public Guid? InterviewId {  get; set; }
        public Guid GivenById { get; private set; }
        public FeedbackStage Stage { get; private set; }
        public string? Comment { get; private set; }
        public int Rating { get; private set; }
        public JobApplication? JobApplication { get; private set; }
        // TODO: add interviw entity mapping here when it is added
        public User GivenByUser { get; private set; } = null!;
        public ICollection<SkillFeedback> SkillFeedbacks { get; private set; } =
            new HashSet<SkillFeedback>();

        public static Feedback Create(
            Guid? id,
            Guid? jobApplicationId,
            Guid givenById,
            FeedbackStage stage,
            string? comment,
            int rating,
            IEnumerable<SkillFeedback> skillFeedbacks
        )
        {
            return new Feedback(
                id,
                jobApplicationId,
                givenById,
                stage,
                comment,
                rating,
                skillFeedbacks
            );
        }

        public void Update(
            string? comment,
            int rating,
            IEnumerable<SkillFeedback> skillFeedbacks
        )
        {
            Comment = comment;
            Rating = rating;

            SyncSkillFeedbacks(skillFeedbacks);
        }

        private void SyncSkillFeedbacks(IEnumerable<SkillFeedback> newItems)
        {
            if (newItems is null) return;

            // remove
            foreach (var existing in SkillFeedbacks.ToList())
            {
                if (!newItems.Any(x => x.SkillId == existing.SkillId))
                    SkillFeedbacks.Remove(existing);
            }

            // update
            foreach (var incoming in newItems)
            {
                var toUpdate = SkillFeedbacks.FirstOrDefault(x => x.SkillId == incoming.SkillId);
                toUpdate?.Update(
                        rating: incoming.Rating,
                        assessedExpYears: incoming.AssessedExpYears
                    );
            }

            // add
            foreach (var item in newItems)
            {
                if (!SkillFeedbacks.Any(x => x.SkillId == item.SkillId))
                    SkillFeedbacks.Add(item);
            }
        }
    }
}