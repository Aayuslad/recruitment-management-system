using Server.Core.Primitives;
using Server.Domain.Entities.Abstractions;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class JobApplication : AuditableEntity, IAggregateRoot
    {
        private JobApplication() : base(Guid.Empty, Guid.Empty) { }

        private JobApplication(
            Guid? id,
            Guid createdBy,
            Guid candidateId,
            Guid jobOpeningId
        ) : base(id ?? Guid.NewGuid(), createdBy)
        {
            CandidateId = candidateId;
            JobOpeningId = jobOpeningId;
            AppliedAt = DateTime.UtcNow;
            Status = JobApplicationStatus.Applied;
        }

        public Guid CandidateId { get; private set; }
        public Guid JobOpeningId { get; private set; }
        public DateTime AppliedAt { get; private set; }
        public JobApplicationStatus Status { get; private set; }
        public Candidate Candidate { get; private set; } = null!;
        public JobOpening JobOpening { get; private set; } = null!;
        public ICollection<Feedback> Feedbacks { get; private set; } =
            new HashSet<Feedback>();
        public ICollection<JobApplicationStatusMoveHistory> StatusMoveHistories { get; private set; } =
            new HashSet<JobApplicationStatusMoveHistory>();

        public static JobApplication Create(
            Guid? id,
            Guid createdBy,
            Guid candidateId,
            Guid jobOpeningId
        )
        {
            return new JobApplication(
                id,
                createdBy,
                candidateId,
                jobOpeningId
            );
        }

        public void Delete(Guid deletedBy)
        {
            MarkAsDeleted(deletedBy);
        }

        public void AddFeedback(Feedback feedback)
        {
            if (feedback is null) return;

            if (!Feedbacks.Any(x => x.Id == feedback.Id))
                Feedbacks.Add(feedback);
        }

        public void UpdateFeedback(
            Guid feedbackId,
            string? comment,
            int rating,
            IEnumerable<SkillFeedback> skillFeedbacks
        )
        {
            var existing = Feedbacks.FirstOrDefault(x => x.Id == feedbackId);
            if (existing is null) return;

            existing.Update(
                comment: comment,
                rating: rating,
                skillFeedbacks: skillFeedbacks
            );
        }

        public void DeleteFeedback(Guid feedbackId)
        {
            var existing = Feedbacks.FirstOrDefault(x => x.Id == feedbackId);
            if (existing is null) return;

            Feedbacks.Remove(existing);
        }

        public void MoveStatus(Guid doneById, JobApplicationStatus moveTo)
        {
            var moveHistory = JobApplicationStatusMoveHistory.Create(
                    id: null,
                    jobApplicationId: Id,
                    statusMovedTo: moveTo,
                    movedById: doneById,
                    comment: null
                );
            StatusMoveHistories.Add(moveHistory);

            Status = moveTo;
        }
    }
}