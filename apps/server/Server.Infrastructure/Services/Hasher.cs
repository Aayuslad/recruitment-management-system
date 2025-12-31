using Microsoft.AspNetCore.Identity;

using Server.Application.Abstractions.Services;

namespace Server.Infrastructure.Services
{
    public class Hasher : IHasher
    {
        private readonly PasswordHasher<string> _passwordHasher = new();

        public Hasher() { }

        public string Hash(string password)
        {
            return _passwordHasher.HashPassword(string.Empty, password);
        }

        public bool Verify(string hashedPassword, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(string.Empty, hashedPassword, password);

            return result == PasswordVerificationResult.Success;
        }
    }
}