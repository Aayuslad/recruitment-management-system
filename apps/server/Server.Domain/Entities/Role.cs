using Server.Core.Entities;
using Server.Core.Primitives;

namespace Server.Domain.Entities
{
    public class Role : BaseEntity<Guid>, IAggregateRoot
    {
        private Role() : base(Guid.Empty) { }

        private Role(Guid id, string name, string? description) : base(id)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }

        public static Role Create(string name, string? description = null)
        {
            return new Role(Guid.NewGuid(), name, description);
        }
    }
}