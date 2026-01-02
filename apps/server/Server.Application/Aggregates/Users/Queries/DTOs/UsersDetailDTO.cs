using Server.Domain.Enums;

namespace Server.Application.Aggregates.Users.Queries.DTOs
{
    public class UsersDetailDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public UserStatus Status { get; set; }
        public string ContactNumber { get; set; } = null!;
        public bool IsContactNumberVerified { get; set; }
        public Gender Gender { get; set; }
        public DateTime Dob { get; set; }
        public List<UserRolesDTO> Roles { get; set; } = new List<UserRolesDTO>();
    }
}