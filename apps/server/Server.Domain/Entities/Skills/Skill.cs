using Server.Core.Primitives;
using Server.Domain.Entities.Abstractions;

namespace Server.Domain.Entities.Skills
{
    public class Skill : AuditableEntity, IAggregateRoot
    {
        private Skill() : base(Guid.Empty, Guid.Empty) { }

        private Skill(Guid id, string name, Guid createdBy)
            : base(id, createdBy)
        {
            Name = name;
        }

        public string Name { get; private set; } = null!;

        public void Update(string name, Guid updatedBy)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");
            if (updatedBy == Guid.Empty)
                throw new ArgumentException("Invalid UpdatedBy user.");

            Name = name;

            MarkAsUpdated(updatedBy);
        }

        public void Delete(Guid deletedBy)
        {
            if (deletedBy == Guid.Empty)
                throw new ArgumentException("Invalid DeletedBy user.");

            MarkAsDeleted(deletedBy);
        }

        public static Skill Create(string name, Guid createdBy)
        {
            return new Skill(Guid.NewGuid(), name, createdBy);
        }
    }
}