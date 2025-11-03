namespace Server.Domain.Entities
{
    public class PositionBatchReviewers
    {
        private PositionBatchReviewers() { }
        private PositionBatchReviewers(Guid positionBatchId, Guid reviewerUserId)
        {
            PositionBatchId = positionBatchId;
            ReviewerUserId = reviewerUserId;
        }

        public Guid PositionBatchId { get; private set; }
        public Guid ReviewerUserId { get; private set; }
        public PositionBatch PositionBatch { get; private set; } = default!;
        public User ReviewerUser { get; private set; } = default!;

        public static PositionBatchReviewers Create(Guid positionBatchId, Guid reviewerUserId)
        {
            return new PositionBatchReviewers(positionBatchId, reviewerUserId);
        }
    }
}