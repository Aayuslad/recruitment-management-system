using Server.Core.Entities;

namespace Server.Domain.Entities
{
    public class Notification : BaseEntity<Guid>
    {
        private Notification() : base(Guid.Empty) { }

        private Notification(
            Guid? id,
            Guid userId,
            Guid? fromUserId,
            string subject,
            string message,
            bool isRead
        ) : base(id ?? Guid.NewGuid())
        {
            UserId = userId;
            FromUserId = fromUserId;
            Subject = subject;
            Message = message;
            IsRead = isRead;
        }

        public Guid UserId { get; private set; }
        public Guid? FromUserId { get; private set; }
        public string Subject { get; private set; } = null!;
        public string Message { get; private set; } = null!;
        public bool IsRead { get; private set; }
        public User User { get; private set; } = null!;
        public User FromUser { get; private set; } = null!;

        public static Notification Create(
            Guid? id,
            Guid userId,
            Guid? fromUserId,
            string subject,
            string message
        )
        {
            return new Notification(
                id,
                userId,
                fromUserId,
                subject,
                message,
                false
            );
        }


        public void MarkAsRead()
        {
            if (!IsRead)
                IsRead = true;
        }
    }
}