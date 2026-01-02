namespace Server.Core.Entities
{
    /// <summary>
    /// Base entity implementing the surrogate key
    /// </summary>
    public abstract class BaseEntity<Guid> : IEntity<Guid>
    {
        protected BaseEntity(Guid id)
        {
            Id = id;
        }

        public virtual Guid Id { get; protected set; }

        public override bool Equals(object? obj)
        {
            if (obj is not BaseEntity<Guid> other) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<Guid>.Default.Equals(Id, other.Id);
        }

        private static bool IsTransient(Guid id)
        {
            return EqualityComparer<Guid>.Default.Equals(id, default!);
        }

        public override int GetHashCode()
        {
            // If transient use base implementation (object hash) to avoid collisions
            return IsTransient(Id) ? base.GetHashCode() : Id!.GetHashCode();
        }
    }
}