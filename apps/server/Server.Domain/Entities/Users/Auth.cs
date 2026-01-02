using Server.Domain.Entities.Abstractions;
using Server.Domain.ValueObjects;

namespace Server.Domain.Entities.Users
{
    public class Auth : AuditableEntity
    {
        private Auth() : base(Guid.Empty, Guid.Empty) { }

        private Auth(
            Guid id,
            string userName,
            Email email,
            string passwordHash,
            Guid? createdBy
        ) : base(id, createdBy)
        {
            Email = email;
            UserName = userName;
            PasswordHash = passwordHash;
        }

        public string UserName { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public string? PasswordHash { get; private set; }
        public DateTime? LastLoginAt { get; private set; }

        public void UpdatePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            MarkAsUpdated(Guid.Empty);
        }

        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }

        public void Delete(Guid deletedBy)
        {
            if (deletedBy == Guid.Empty)
                throw new ArgumentException("Invalid DeletedBy user.");

            MarkAsDeleted(deletedBy);
        }

        public static Auth Create(
            string userName,
            Email email,
            string passwordHash,
            Guid? createdBy
        )
        {
            return new Auth(
                Guid.NewGuid(),
                userName,
                email,
                passwordHash,
                createdBy
            );
        }
    }
}