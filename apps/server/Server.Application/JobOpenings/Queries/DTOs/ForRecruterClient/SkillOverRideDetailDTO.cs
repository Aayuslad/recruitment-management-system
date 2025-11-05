using Server.Domain.Enums;

namespace Server.Application.JobOpenings.Queries.DTOs.ForRecruterClient
{
    public class SkillOverRideDetailDTO
    {
        public Guid Id { get; set; }
        public Guid SkillId { get; set; }
        public string? Comments { get; set; } = string.Empty;
        public float MinExperienceYears { get; set; }
        public SkillType Type { get; set; }
        public SkillActionType ActionType { get; set; }
    }
}