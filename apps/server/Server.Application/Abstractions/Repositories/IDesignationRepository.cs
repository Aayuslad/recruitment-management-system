using Server.Domain.Entities.Designations;

namespace Server.Application.Abstractions.Repositories
{
    public interface IDesignationRepository
    {
        Task AddAsync(Designation designation, CancellationToken cancellationToken);
        Task UpdateAsync(Designation designation, CancellationToken cancellationToken);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
        Task<Designation?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Designation>> GetAllAsync(CancellationToken cancellationToken);
    }
}