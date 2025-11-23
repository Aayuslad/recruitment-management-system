namespace Server.Application.Aggregates.Users.Commands.DTOs
{
    public class LoginUserDTO
    {
        public string Token { get; set; } = null!;
        public bool IsProfileCompleted { get; set; }
    }
}