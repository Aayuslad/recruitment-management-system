using Server.Domain.Entities;

namespace Server.Application.Abstractions.Repositories
{
    public interface IRolesRepository
    {
        Task AddAsync(Role role, CancellationToken cancellationToken);
        Task UpdateAsync(Role role, CancellationToken cancellationToken);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
        Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellation);
        Task<List<Role>> GetAllAsync(CancellationToken cancellationToken);
    }
}