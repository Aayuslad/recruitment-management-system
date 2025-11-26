using Server.Core.Primitives;
using Server.Domain.Entities.Abstractions;
using Server.Domain.Enums;
using Server.Domain.ValueObjects;

namespace Server.Domain.Entities.Users
{
    public class User : AuditableEntity, IAggregateRoot
    {
        private User() : base(Guid.Empty, Guid.Empty) { }

        private User(
            Guid id,
            Guid authId,
            string firstName,
            string? middleName,
            string lastName,
            UserStatus? status,
            ContactNumber contactNumber,
            Gender? gender,
            DateTime dob,
            Guid? createdBy
        ) : base(id, createdBy)
        {
            AuthId = authId;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Status = status ?? UserStatus.Active;
            ContactNumber = contactNumber;
            IsContactNumberVerified = false;
            Gender = gender ?? Gender.PreferNotToSay;
            Dob = dob;
        }

        public Guid AuthId { get; private set; }
        public string FirstName { get; private set; } = default!;
        public string? MiddleName { get; private set; }
        public string LastName { get; private set; } = default!;
        public UserStatus Status { get; private set; }
        public ContactNumber ContactNumber { get; private set; } = default!;
        public bool IsContactNumberVerified { get; private set; }
        public Gender Gender { get; private set; }
        public DateTime Dob { get; private set; }
        public Auth Auth { get; private set; } = default!;
        public ICollection<UserRole> Roles { get; private set; } = null!;

        public void Delete(Guid deletedBy)
        {
            if (deletedBy == Guid.Empty)
                throw new ArgumentException("Invalid DeletedBy user.");

            MarkAsDeleted(deletedBy);
        }

        public static User Create(
            Guid authId,
            string firstName,
            string? middleName,
            string lastName,
            UserStatus? status,
            ContactNumber contactNumber,
            Gender? gender,
            DateTime dob,
            Guid? createdBy = null,
            Guid? id = null
        )
        {
            return new User(
                id ?? Guid.NewGuid(),
                authId,
                firstName,
                middleName,
                lastName,
                status,
                contactNumber,
                gender,
                dob,
                createdBy
            );
        }

        public void SyncRoles(IEnumerable<UserRole> newRoles)
        {
            if (newRoles is null) return;

            // remove removed ones
            foreach (var role in Roles.ToList())
            {
                if (!newRoles.Any(x => x.RoleId == role.RoleId))
                    Roles.Remove(role);
            }

            // add added ones
            foreach (var role in newRoles)
            {
                if (!Roles.Any(x => x.RoleId == role.RoleId))
                    Roles.Add(role);
            }
        }
    }
}