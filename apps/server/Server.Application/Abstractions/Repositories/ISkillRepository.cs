using Server.Domain.Entities.Skills;

namespace Server.Infrastructure.Repositories
{
    public interface ISkillRepository
    {
        Task AddAsync(Skill skill, CancellationToken ct);
        Task UpdateAsync(Skill skill, CancellationToken ct);
        Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
        Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct);
        Task<Skill?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<List<Skill>> GetAllAsync(CancellationToken ct);
    }
}