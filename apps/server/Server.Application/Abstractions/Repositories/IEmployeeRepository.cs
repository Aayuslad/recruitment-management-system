using Server.Domain.Entities.Employees;

namespace Server.Application.Abstractions.Repositories
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee, CancellationToken cancellationToken);
        Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken);
    }
}