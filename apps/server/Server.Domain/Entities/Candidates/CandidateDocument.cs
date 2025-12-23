using Server.Core.Entities;
using Server.Domain.Entities.Documents;
using Server.Domain.Entities.Users;

namespace Server.Domain.Entities.Candidates
{
    public class CandidateDocument : BaseEntity<Guid>
    {
        private CandidateDocument() : base(Guid.Empty) { }

        private CandidateDocument(
            Guid? id,
            Guid candidateId,
            Guid documentTypeId,
            string url
        ) : base(id ?? Guid.NewGuid())
        {
            CandidateId = candidateId;
            DocumentTypeId = documentTypeId;
            Url = url;
            IsVerified = false;
        }

        public Guid CandidateId { get; private set; }
        public Guid DocumentTypeId { get; private set; }
        public string Url { get; private set; } = null!;
        public bool IsVerified { get; private set; }
        public Guid? VerifiedById { get; private set; }
        public User? VerifiedByUser { get; private set; }
        public Candidate Candidate { get; private set; } = null!;
        public DocumentType DocumentType { get; private set; } = null!;

        public static CandidateDocument Create(
            Guid? id,
            Guid candidateId,
            Guid documentTypeId,
            string url
        )
        {
            return new CandidateDocument(
                id,
                candidateId,
                documentTypeId,
                url
            );
        }

        public void MarkVerified(Guid verifiedById)
        {
            IsVerified = true;
            VerifiedById = verifiedById;
        }
    }
}