using Server.Domain.Entities.Employees;

namespace Server.Application.Abstractions.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken);
    }
}