namespace Server.Domain.Entities
{
    public class UserRole
    {
        private UserRole() { }

        private UserRole(Guid userId, Guid roleId, Guid assignedBy)
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
        public User AssignedByUser { get; private set; } = null!;

        public static UserRole Create(Guid userId, Guid roleId, Guid assignedBy)
        {
            return new UserRole(userId, roleId, assignedBy);
        }
    }
}