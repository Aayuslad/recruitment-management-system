namespace Server.Application.Aggregates.Designations.Queries.DTOs
{
    public class DesignationDetailDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public List<DesignationSkillDetailDTO>? DesignationSkills { get; set; } = new();
    }
}