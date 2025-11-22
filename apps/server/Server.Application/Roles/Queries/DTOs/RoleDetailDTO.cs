namespace Server.Application.Roles.Queries.DTOs
{
    public class RoleDetailDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
