using Server.Domain.Enums;

namespace Server.Application.Aggregates.Designations.Commands.DTOs
{
    public class DesignationSkillDTO
    {
        public Guid SkillId { get; set; }
        public SkillType SkillType { get; set; }
    }
}