using Server.Domain.ValueObjects;

namespace Server.Application.Aggregates.Employees.Queries.DTOs
{
    public class EmployeeDetailDTO
    {
        public Guid Id { get; set; }
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; } = null!;
        public Email Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public ContactNumber ContactNumber { get; set; } = null!;
        public DateTime Dob { get; set; }
    }
}