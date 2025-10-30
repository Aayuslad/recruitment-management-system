namespace Server.Application.Skills.Queries.DTOs
{
    public class SkillDetailDTO
    {
        public Guid Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public Guid? CreatedBy { get; set; } = default!;
        //public string CreatedByUserName { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = default!;
        public Guid? LastUpdatedBy { get; set; } = default!;
        //public string? LastUpdatedByUserName { get; set; } = default!;
        public DateTime? LastUpdatedAt { get; set; } = default!;
    }
}