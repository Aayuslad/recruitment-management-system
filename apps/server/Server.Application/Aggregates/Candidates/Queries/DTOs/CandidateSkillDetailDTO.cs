namespace Server.Application.Aggregates.Candidates.Queries.DTOs
{
    public class CandidateSkillDetailDTO
    {
        public Guid SkillId { get; set; }
        public string SkillName { get; set; } = null!;
    }
}