using Server.Core.Entities;
using Server.Core.Primitives;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class AuditLog : BaseEntity<Guid>, IAggregateRoot
    {
        private AuditLog() : base(Guid.Empty) { }

        private AuditLog(Guid id, AuditEntityType entityType, string entityId, AuditActionType action, Guid changedBy)
            : base(id)
        {
            EntityType = entityType;
            EntityId = entityId;
            Action = action;
            ChangedBy = changedBy;
            ChangedAt = DateTime.UtcNow;
        }

        public AuditEntityType EntityType { get; private set; }
        public string EntityId { get; private set; } = default!;
        public AuditActionType Action { get; private set; }
        public Guid ChangedBy { get; private set; }
        public DateTime ChangedAt { get; private set; }
        public User CreatedByUser { get; private set; } = default!;

        public static AuditLog Create(AuditEntityType entityType, string entityId, AuditActionType action, Guid changedBy)
        {
            return new AuditLog(Guid.NewGuid(), entityType, entityId, action, changedBy);
        }
    }
}