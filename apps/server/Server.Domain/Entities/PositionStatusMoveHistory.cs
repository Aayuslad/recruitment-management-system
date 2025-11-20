using Server.Core.Entities;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class PositionStatusMoveHistory : BaseEntity<Guid>
    {
        private PositionStatusMoveHistory() : base(Guid.Empty) { }

        private PositionStatusMoveHistory(
            Guid positionId,
            PositionStatus movedTo,
            string? comments,
            Guid? movedById
        ) : base(Guid.NewGuid())
        {
            PositionId = positionId;
            MovedTo = movedTo;
            Comments = comments;
            MovedAt = DateTime.UtcNow;
            MovedById = movedById;
        }

        public Guid PositionId { get; private set; }
        public PositionStatus MovedTo { get; private set; }
        public string? Comments { get; private set; }
        public DateTime MovedAt { get; private set; }
        public Guid? MovedById { get; private set; }
        public Position Position { get; private set; } = null!;
        public User MovedByUser { get; private set; } = null!;

        public static PositionStatusMoveHistory Create(
            Guid positionId,
            PositionStatus movedTo,
            string? comments,
            Guid? movedById
        )
        {
            return new PositionStatusMoveHistory(
                positionId,
                movedTo,
                comments,
                movedById
            );
        }
    }
}