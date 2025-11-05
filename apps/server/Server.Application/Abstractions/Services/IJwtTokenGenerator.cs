namespace Server.Application.Abstractions.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid authId, Guid? userId, string userName);
        bool ValidateToken(string token);
    }
}