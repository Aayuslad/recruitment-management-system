using Server.Core.Entities;
using Server.Core.Primitives;

namespace Server.Domain.Entities
{
    public class Role : BaseEntity<Guid>, IAggregateRoot
    {
        private Role() : base(Guid.Empty) { }

        private Role(Guid id, string roleName, string? description) : base(id)
        {
            RoleName = roleName;
            Description = description;
        }

        public string RoleName { get; private set; } = default!;
        public string? Description { get; private set; }

        public static Role Create(string roleName, string? description = null)
        {
            return new Role(Guid.NewGuid(), roleName, description);
        }
    }
}