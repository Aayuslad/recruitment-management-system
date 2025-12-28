using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobOpenings.Commands.DTOs
{
    public class SkillOverRideDTO
    {
        public Guid? Id { get; set; }
        public Guid SkillId { get; set; }
        public string? Comments { get; set; } = string.Empty;
        public SkillType Type { get; set; }
        public SkillActionType ActionType { get; set; }
    }
}