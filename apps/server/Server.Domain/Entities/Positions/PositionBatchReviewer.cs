using Server.Domain.Entities.Users;

namespace Server.Domain.Entities.Positions
{
    public class PositionBatchReviewer
    {
        private PositionBatchReviewer() { }

        private PositionBatchReviewer(Guid positionBatchId, Guid reviewerId)
        {
            PositionBatchId = positionBatchId;
            ReviewerId = reviewerId;
        }

        public Guid PositionBatchId { get; private set; }
        public Guid ReviewerId { get; private set; }
        public PositionBatch PositionBatch { get; private set; } = default!;
        public User ReviewerUser { get; private set; } = default!;

        public static PositionBatchReviewer Create(Guid positionBatchId, Guid reviewerId)
        {
            return new PositionBatchReviewer(positionBatchId, reviewerId);
        }
    }
}