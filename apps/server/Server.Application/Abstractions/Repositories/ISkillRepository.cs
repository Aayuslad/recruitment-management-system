using Server.Domain.Entities;

namespace Server.Infrastructure.Repositories
{
    public interface ISkillRepository
    {
        Task AddAsync(Skill skill, CancellationToken cancellation);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
        Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Skill?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Skill>> GetAllAsync(CancellationToken cancellationToken);
        Task UpdateAsync(Skill skill, CancellationToken cancellation);
    }
}
