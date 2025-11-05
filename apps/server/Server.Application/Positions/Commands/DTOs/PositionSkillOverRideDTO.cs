using Server.Domain.Enums;

namespace Server.Application.Positions.Commands.DTOs
{
    public class PositionSkillOverRideDTO
    {
        public Guid SkillId { get; set; }
        public string? Comments { get; set; } = string.Empty;
        public float MinExperienceYears { get; set; }
        public SkillType Type { get; set; }
        public SkillActionType ActionType { get; set; }
    }
}