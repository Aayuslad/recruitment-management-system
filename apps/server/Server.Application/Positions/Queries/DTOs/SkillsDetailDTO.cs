using Server.Domain.Enums;

namespace Server.Application.Positions.Queries.DTOs
{
    public class SkillsDetailDTO
    {
        public Guid SkillId { get; set; }
        public string SkillName { get; set; } = null!;
        public SkillType SkillType { get; set; }
        public float? MinExperienceYears { get; set; }
    }
}