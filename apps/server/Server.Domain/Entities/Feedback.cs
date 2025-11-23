using Server.Core.Entities;
using Server.Domain.Entities.Interviews;
using Server.Domain.Entities.JobApplications;
using Server.Domain.Entities.Users;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class Feedback : BaseEntity<Guid>
    {
        private Feedback() : base(Guid.Empty) { }

        private Feedback(
            Guid? id,
            Guid? jobApplicationId,
            Guid? interviewId,
            Guid givenById,
            FeedbackStage stage,
            string? comment,
            int rating,
            IEnumerable<SkillFeedback> skillFeedbacks
        ) : base(id ?? Guid.NewGuid())
        {
            JobApplicationId = jobApplicationId;
            InterviewId = interviewId;
            GivenById = givenById;
            Stage = stage;
            Comment = comment;
            Rating = rating;

            SkillFeedbacks = skillFeedbacks.ToHashSet();
        }

        public Guid? JobApplicationId { get; private set; }
        public Guid? InterviewId { get; set; }
        public Guid GivenById { get; private set; }
        public FeedbackStage Stage { get; private set; }
        public string? Comment { get; private set; }
        public int Rating { get; private set; }
        public JobApplication? JobApplication { get; private set; }
        public Interview? Interview { get; private set; } = null;
        public User GivenByUser { get; private set; } = null!;
        public ICollection<SkillFeedback> SkillFeedbacks { get; private set; } =
            new HashSet<SkillFeedback>();

        public static Feedback CreateForReviewStage(
            Guid? id,
            Guid? jobApplicationId,
            Guid givenById,
            string? comment,
            int rating,
            IEnumerable<SkillFeedback> skillFeedbacks
        )
        {
            return new Feedback(
                id,
                jobApplicationId,
                null,
                givenById,
                FeedbackStage.Review,
                comment,
                rating,
                skillFeedbacks
            );
        }

        public static Feedback CreateForInterviewStage(
            Guid? id,
            Guid? interviewId,
            Guid givenById,
            string? comment,
            int rating,
            IEnumerable<SkillFeedback> skillFeedbacks
        )
        {
            return new Feedback(
                id,
                interviewId,
                null,
                givenById,
                FeedbackStage.Interview,
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