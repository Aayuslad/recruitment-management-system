using Server.Domain.Enums;

namespace Server.Application.JobOpenings.Queries.DTOs.ForCandiateClient
{
    public class JobOpeningDetailDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string DesignationName { get; set; } = null!;
        public JobOpeningType Type { get; set; }
        public List<SkillDetailDTO> Skills { get; set; } = new List<SkillDetailDTO>();
    }
}