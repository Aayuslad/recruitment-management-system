using Microsoft.EntityFrameworkCore;

using Server.Domain.Entities.Skills;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class SkillRepository : ISkillRepository
    {
        private readonly ApplicationDbContext _context;

        public SkillRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task ISkillRepository.AddAsync(Skill skill, CancellationToken cancellationToken)
        {
            _context.Skills.Add(skill);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<bool> ISkillRepository.ExistsByNameAsync(string name, CancellationToken cancellationToken)
        {
            return _context.Skills.AnyAsync(x => x.Name == name, cancellationToken);
        }

        Task<bool> ISkillRepository.ExistsByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Skills.AnyAsync(x => x.Id == id, cancellationToken);
        }

        Task<Skill?> ISkillRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Skills
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        Task<List<Skill>> ISkillRepository.GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.Skills
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        Task ISkillRepository.UpdateAsync(Skill skill, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}