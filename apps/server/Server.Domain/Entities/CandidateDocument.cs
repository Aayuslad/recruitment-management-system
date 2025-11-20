using Server.Core.Entities;

namespace Server.Domain.Entities
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
        public Guid? VerifiedBy { get; private set; }
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

        public void MarkVerified(Guid verifiedBy)
        {
            IsVerified = true;
            VerifiedBy = verifiedBy;
        }
    }
}