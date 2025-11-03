using Server.Core.Entities;

namespace Server.Domain.Entities
{
    public class UserRole : BaseEntity<Guid>
    {
        private UserRole() : base(Guid.Empty) { }

        private UserRole(Guid id, Guid userId, Guid roleId, Guid assignedBy) : base(id)
        {
            UserId = userId;
            RoleId = roleId;
            AssignedBy = assignedBy;
            AssignedAt = DateTime.UtcNow;
        }

        public Guid UserId { get; private set; }
        public Guid RoleId { get; private set; }
        public Guid AssignedBy { get; private set; }
        public DateTime AssignedAt { get; private set; }
        public User User { get; private set; } = null!;
        public Role Role { get; private set; } = null!;

        public static UserRole Assign(Guid userId, Guid roleId, Guid assignedBy)
        {
            return new UserRole(Guid.NewGuid(), userId, roleId, assignedBy);
        }
    }
}