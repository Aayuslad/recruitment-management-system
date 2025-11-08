using Server.Core.Primitives;

namespace Server.Domain.Entities
{
    public class Candidate : AuditableEntity, IAggregateRoot
    {
        private Candidate() : base(Guid.Empty, Guid.Empty) { }
    }
}