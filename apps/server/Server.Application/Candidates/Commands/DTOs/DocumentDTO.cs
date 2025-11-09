namespace Server.Application.Candidates.Commands.DTOs
{
    public class DocumentDTO
    {
        public Guid? Id { get; set; }
        public Guid DocumentTypeId { get; set; }
        public string Url { get; set; } = null!;
    }
}