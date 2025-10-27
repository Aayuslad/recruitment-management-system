namespace Server.Core.Entities
{
    public abstract class AuditableEntity : BaseEntity<Guid>
    {
        public Guid? CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid? LastUpdatedBy { get; private set; }
        public DateTime? LastUpdatedAt { get; private set; }
        public bool IsDeleted { get; private set; } = false;
        public Guid? DeletedBy { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        protected AuditableEntity(Guid id, Guid createdBy) : base(id)
        {
            CreatedBy = createdBy == Guid.Empty ? Guid.Empty : createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        protected void SetUpdated(Guid updatedBy)
        {
            LastUpdatedBy = updatedBy;
            LastUpdatedAt = DateTime.UtcNow;
        }

        protected void MarkAsDeleted(Guid deletedBy)
        {
            IsDeleted = true;
            DeletedBy = deletedBy;
            DeletedAt = DateTime.UtcNow;
        }
    }
}