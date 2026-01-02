using Server.Core.Entities;
using Server.Core.Primitives;
using Server.Domain.Entities.Designations;
using Server.Domain.ValueObjects;

namespace Server.Domain.Entities.Employees
{
    public class Employee : BaseEntity<Guid>, IAggregateRoot
    {
        private Employee() : base(Guid.Empty) { }

        private Employee(
            Guid? id,
            Guid designationId,
            Email email,
            string firstName,
            string? middleName,
            string lastName,
            ContactNumber contactNumber,
            DateTime dob
        ) : base(id ?? Guid.NewGuid())
        {
            DesignationId = designationId;
            Email = email;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            ContactNumber = contactNumber;
            Dob = dob;
        }

        public Guid DesignationId { get; set; }
        public Email Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public ContactNumber ContactNumber { get; set; } = null!;
        public DateTime Dob { get; set; }
        public Designation Designation { get; set; } = null!;

        // create factory fun
        public static Employee Create(
            Guid? id,
            Guid designationId,
            Email email,
            string firstName,
            string? middleName,
            string lastName,
            ContactNumber contactNumber,
            DateTime dob
        ) => new Employee(
            id ?? Guid.NewGuid(),
            designationId,
            email,
            firstName,
            middleName,
            lastName,
            contactNumber,
            dob
        );
    }
}