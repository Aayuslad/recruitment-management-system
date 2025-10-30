namespace Server.Application.Designations.Queries.DTOs
{
    public class DesignationDetailDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public List<DesignationSkillDetailDTO>? DesignationSkills { get; set; } = new();
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}