using Server.Core.Primitives;
using Server.Domain.Entities.Abstractions;

namespace Server.Domain.Entities
{
    public class Role : AuditableEntity , IAggregateRoot
    {
        private Role() : base(Guid.Empty, Guid.Empty) { }

        private Role(
            string name, 
            string? description, 
            Guid createdBy
        ) : base(Guid.NewGuid(), createdBy)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }

        public static Role Create(
            string name, 
            string? description,
            Guid createdBy
        )
        {
            return new Role(
                name, 
                description,
                createdBy
            );
        }

        public void Update(
            string name, 
            string? description,
            Guid updatedBy
        )
        {
            Name = name;
            Description = description;

            MarkAsUpdated(updatedBy);
        }


        public void Delete(Guid deletedBy)
        {
            MarkAsDeleted(deletedBy);
        }
    }
}