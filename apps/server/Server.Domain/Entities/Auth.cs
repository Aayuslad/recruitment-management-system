using Server.Core.Entities;
using Server.Core.Primitives;
using Server.Domain.ValueObjects;

namespace Server.Domain.Entities
{
    /// <summary>
    /// Auth entity representing user/candidate authentication details.
    /// </summary>
    public class Auth : BaseEntity<Guid>, IAggregateRoot
    {
        private Auth() : base(Guid.Empty) { }

        public Auth(Guid id, string userName, Email email, string passwordHash, string googleId) : base(id)
        {
            Email = email;
            UserName = userName;
            PasswordHash = passwordHash;
            GoogleId = googleId;
        }

        public string UserName { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public string? PasswordHash { get; private set; }
        public string? GoogleId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public void UpdatePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }

        /// <summary>
        /// factory method to create a new Auth entity
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passwordHash"></param>
        /// <param name="googleId"></param>
        /// <returns></returns>
        public static Auth Create(string userName, Email email, string passwordHash, string googleId)
        {
            return new Auth(Guid.NewGuid(), userName, email, passwordHash, googleId);
        }
    }
}