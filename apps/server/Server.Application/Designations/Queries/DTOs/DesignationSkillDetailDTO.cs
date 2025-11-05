using Server.Domain.Enums;

namespace Server.Application.Designations.Queries.DTOs
{
    public class DesignationSkillDetailDTO
    {
        public Guid SkillId { get; set; }
        public string Name { get; set; } = default!;
        public SkillType SkillType { get; set; }
        public float? MinExperienceYears { get; set; }
    }
}