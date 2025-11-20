using Server.Core.Entities;

namespace Server.Domain.Entities
{
    public class DocumentType : BaseEntity<Guid>
    {
        private DocumentType() : base(Guid.Empty) { }

        public string Name { get; private set; } = null!;
    }
}