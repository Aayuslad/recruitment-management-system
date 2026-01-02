namespace Server.Application.Aggregates.Users.Commands.DTOs
{
    public class UserRolesDTO
    {
        public Guid RoleId { get; set; }
        public Guid? AssignedBy { get; set; }
    }
}