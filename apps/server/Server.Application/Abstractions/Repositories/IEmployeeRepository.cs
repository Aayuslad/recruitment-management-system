using Server.Domain.Entities;

namespace Server.Application.Abstractions.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken);
    }
}