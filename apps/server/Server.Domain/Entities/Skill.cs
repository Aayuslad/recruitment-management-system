using Server.Core.Entities;
using Server.Core.Primitives;

namespace Server.Domain.Entities
{
    public class Skill : AuditableEntity, IAggregateRoot
    {
        private Skill() : base(Guid.Empty, Guid.Empty) { }

        private Skill(Guid id, string name, string description, Guid createdBy)
            : base(id, createdBy)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;

        public void UpdateDetails(string name, string description, Guid updatedBy)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty.");
            if (updatedBy == Guid.Empty)
                throw new ArgumentException("Invalid UpdatedBy user.");

            Name = name;
            Description = description;

            SetUpdated(updatedBy);
        }

        public void Delete(Guid deletedBy)
        {
            if (deletedBy == Guid.Empty)
                throw new ArgumentException("Invalid DeletedBy user.");

            MarkAsDeleted(deletedBy);
        }

        public static Skill Create(string name, string description, Guid createdBy)
        {
            return new Skill(Guid.NewGuid(), name, description, createdBy);
        }
    }
}
