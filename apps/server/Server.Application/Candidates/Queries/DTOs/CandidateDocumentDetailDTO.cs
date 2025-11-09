namespace Server.Application.Candidates.Queries.DTOs
{
    public class CandidateDocumentDetailDTO
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public Guid DocumentTypeId { get; set; }
        public string DocumentName { get; set; } = null!;
        public bool IsVerified { get; set; }
        public Guid? VerifiedBy { get; set; }
    }
}