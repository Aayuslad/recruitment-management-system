using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    public class DesignationRepository : IDesignationRepository
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
            _context.Designations.Update(designation);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<Designation?> IDesignationRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Designations
                .Include(d => d.DesignationSkills)
                    .ThenInclude(ds => ds.Skill)
                .AsNoTracking()
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
            return Task.FromResult(_context.Designations.Any(x => x.Name == name));
        }
    }
}