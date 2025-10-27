namespace Server.Application.DTOs
{
    public class SkillDTO
    {
        public Guid Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = default!;

        //public string CreatedByUserName { get; set; } = default!;
    }
}
