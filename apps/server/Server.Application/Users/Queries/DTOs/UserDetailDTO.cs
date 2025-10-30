namespace Server.Application.Users.Queries.DTOs
{
    public class UserDetailDTO
    {
        public Guid AuthId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string ContactNumber { get; set; } = default!;
        public bool IsContactNumberVerified { get; set; }
        public string Gender { get; set; } = default!;
        public string Dob { get; set; } = default!;
    }
}