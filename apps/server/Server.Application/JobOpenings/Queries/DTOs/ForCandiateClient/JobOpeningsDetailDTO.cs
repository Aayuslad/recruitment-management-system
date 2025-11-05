using Server.Domain.Enums;

namespace Server.Application.JobOpenings.Queries.DTOs.ForCandiateClient
{
    public class JobOpeningsDetailDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string DesignationName { get; set; } = null!;
        public JobOpeningType Type { get; set; }
    }
}