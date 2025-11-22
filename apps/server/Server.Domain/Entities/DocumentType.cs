using Server.Core.Primitives;
using Server.Domain.Entities.Abstractions;

namespace Server.Domain.Entities
{
    public class DocumentType : AuditableEntity, IAggregateRoot
    {
        private DocumentType() : base(Guid.Empty, Guid.Empty) { }

        private DocumentType(Guid id, string name, Guid createdBy) : base(id, createdBy)
        {
            Name = name;
        }

        public string Name { get; private set; } = null!;

        public static DocumentType Create(string name, Guid createdBy)
        {
            return new DocumentType(Guid.NewGuid(), name, createdBy);
        }

        public void Delete(Guid deletedBy)
        {
            MarkAsDeleted(deletedBy);
        }
    }
}