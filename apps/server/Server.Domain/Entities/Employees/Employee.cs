using Server.Core.Entities;
using Server.Core.Primitives;
using Server.Domain.Entities.Designations;
using Server.Domain.ValueObjects;

namespace Server.Domain.Entities.Employees
{
    public class Employee : BaseEntity<Guid>, IAggregateRoot
    {
        private Employee() : base(Guid.Empty) { }

        public Guid DesignationId { get; set; }
        public Email Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public ContactNumber ContactNumber { get; set; } = null!;
        public DateTime Dob { get; set; }
        public Designation Designation { get; set; } = null!;
    }
}