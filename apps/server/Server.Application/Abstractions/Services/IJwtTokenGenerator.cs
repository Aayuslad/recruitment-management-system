namespace Server.Application.Abstractions.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId, string authId);
        bool ValidateToken(string token);
    }
}