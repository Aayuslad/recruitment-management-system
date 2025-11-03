using Server.Domain.Enums;

namespace Server.Application.Positions.Queries.DTOs.PositionBatchDTOs
{
    public class SkillOverRideDTO
    {
        public Guid Id { get; set; }
        public Guid PositionBatchId { get; set; }
        // add job opening id later
        public Guid SkillId { get; set; }
        public string? Comments { get; set; } = string.Empty;
        public float MinExperienceYears { get; set; }
        public SkillType Type { get; set; }
        public SkillActionType ActionType { get; set; }
    }
}