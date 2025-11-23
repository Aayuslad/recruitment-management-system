using Server.Core.Entities;
using Server.Core.Primitives;
using Server.Domain.Entities.Candidates;
using Server.Domain.Enums;

namespace Server.Domain.Entities.Positions
{
    public class Position : BaseEntity<Guid>, IAggregateRoot
    {
        private Position() : base(Guid.Empty) { }

        private Position(Guid batchId) : base(Guid.NewGuid())
        {
            BatchId = batchId;
            Status = PositionStatus.Open;
        }

        public Guid BatchId { get; private set; }
        public PositionStatus Status { get; private set; }
        public Guid? ClosedByCandidate { get; private set; }
        public string? ClosureReason { get; private set; }
        public Candidate? Candidate { get; private set; }
        public PositionBatch PositionBatch { get; private set; } = null!;
        public ICollection<PositionStatusMoveHistory> StatusMoveHistories { get; private set; } =
            new HashSet<PositionStatusMoveHistory>();

        public static Position Create(Guid batchId)
        {
            return new Position(batchId);
        }

        public void PutOnHold(Guid updatedBy, string? comments)
        {
            if (Status == PositionStatus.OnHold)
            {
                throw new ArgumentException("Invalid status change — already OnHold");
            }

            if (Status == PositionStatus.Closed)
            {
                throw new ArgumentException("Invalid status change — Position is Closed");
            }

            var history = PositionStatusMoveHistory.Create(Id, PositionStatus.OnHold, comments, updatedBy);
            StatusMoveHistories.Add(history);
            Status = PositionStatus.OnHold;
        }

        public void ReOpen(Guid updatedBy, string? comments)
        {
            if (Status == PositionStatus.Open)
            {
                throw new ArgumentException("Invalid status change — already Open");
            }

            var history = PositionStatusMoveHistory.Create(Id, PositionStatus.Open, comments, updatedBy);
            Status = PositionStatus.Open;
            StatusMoveHistories.Add(history);
        }

        public void CloseWithCandidate(Guid closedByCandidate, Guid updatedBy)
        {
            if (Status == PositionStatus.Closed)
            {
                throw new ArgumentException("Invalid status change — already Closed");
            }

            string comments = $"position is closed by candidate id: {closedByCandidate}";
            var history = PositionStatusMoveHistory.Create(Id, PositionStatus.Closed, comments, updatedBy);
            ClosedByCandidate = closedByCandidate;
            Status = PositionStatus.Closed;
            StatusMoveHistories.Add(history);
        }

        public void CloseWithoutCandidate(string closureReason, Guid updatedBy)
        {
            if (Status == PositionStatus.Closed)
            {
                throw new ArgumentException("Invalid status change — already Closed");
            }

            string comments = $"Closed without candidate with reason: {closureReason}";
            var history = PositionStatusMoveHistory.Create(Id, PositionStatus.Closed, comments, updatedBy);
            Status = PositionStatus.Closed;
            ClosureReason = closureReason;
            StatusMoveHistories.Add(history);
        }
    }
}