namespace Server.Application.Abstractions.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid authId, Guid? userId, string? userName, IEnumerable<string>? roles = null);
        bool ValidateToken(string token);
    }
}