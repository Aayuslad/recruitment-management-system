namespace Server.Application.Candidates.Queries.DTOs
{
    public class CandidateSkillDetailDTO
    {
        public Guid SkillId { get; set; }
        public string SkillName { get; set; } = null!;
    }
}