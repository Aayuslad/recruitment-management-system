namespace Server.Application.Abstractions.Services
{
    public interface IHasher
    {
        string Hash(string password);
        bool Verify(string hashedPassword, string password);
    }
}