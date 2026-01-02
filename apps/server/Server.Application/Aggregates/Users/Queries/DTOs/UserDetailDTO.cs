using Server.Domain.Enums;

namespace Server.Application.Aggregates.Users.Queries.DTOs
{
    public class UserDetailDTO
    {
        public bool IsProfileCompleted { get; set; }
        public Guid AuthId { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public UserStatus? Status { get; set; }
        public string? ContactNumber { get; set; }
        public bool? IsContactNumberVerified { get; set; }
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public List<UserRolesDTO> Roles { get; set; } = new List<UserRolesDTO>();
    }
}