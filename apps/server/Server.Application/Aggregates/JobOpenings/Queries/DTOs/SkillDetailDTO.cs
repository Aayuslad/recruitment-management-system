using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobOpenings.Queries.DTOs
{
    public class SkillDetailDTO
    {
        public Guid SkillId { get; set; }
        public string SkillName { get; set; } = null!;
        public SkillType SkillType { get; set; }
    }
}