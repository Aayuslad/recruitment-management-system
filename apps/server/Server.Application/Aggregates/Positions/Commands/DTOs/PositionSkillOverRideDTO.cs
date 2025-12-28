using Server.Domain.Enums;

namespace Server.Application.Aggregates.Positions.Commands.DTOs
{
    public class PositionSkillOverRideDTO
    {
        public Guid? Id { get; set; }
        public Guid SkillId { get; set; }
        public string? Comments { get; set; } = string.Empty;
        public SkillType Type { get; set; }
        public SkillActionType ActionType { get; set; }
    }
}