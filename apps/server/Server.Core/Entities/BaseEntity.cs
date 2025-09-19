using System.Collections.Generic;
using System.Security.Cryptography;

namespace Server.Core.Entities
{
    /// <summary>
    /// Base entity implementing simple identity-based equality semantics.
    /// </summary>
    public abstract class BaseEntity<Guid> : IEntity<Guid>
    {
        public virtual Guid Id { get; protected set; }

        protected BaseEntity(Guid id)
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = (BaseEntity<Guid>)obj;

            // If either entity is transient (default id) they are not equal
            if (IsTransient(Id) || IsTransient(other.Id)) return false;

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

        public static bool operator ==(BaseEntity<Guid>? left, BaseEntity<Guid>? right) => Equals(left, right);
        public static bool operator !=(BaseEntity<Guid>? left, BaseEntity<Guid>? right) => !Equals(left, right);
    }
}