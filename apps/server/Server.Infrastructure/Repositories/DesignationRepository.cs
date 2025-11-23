using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities.Designations;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class DesignationRepository : IDesignationRepository
    {
        private readonly ApplicationDbContext _context;

        public DesignationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task IDesignationRepository.AddAsync(Designation designation, CancellationToken cancellationToken)
        {
            _context.Designations.Add(designation);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task IDesignationRepository.UpdateAsync(Designation designation, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<Designation?> IDesignationRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Designations
                .AsTracking()
                .Include(d => d.DesignationSkills)
                    .ThenInclude(ds => ds.Skill)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        Task<List<Designation>> IDesignationRepository.GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.Designations
                .Include(x => x.DesignationSkills)
                    .ThenInclude(ds => ds.Skill)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        Task<bool> IDesignationRepository.ExistsByNameAsync(string name, CancellationToken cancellationToken)
        {
            return _context.Designations.AnyAsync(x => x.Name == name, cancellationToken);
        }
    }
}