using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class RolesRepository : IRolesRepository
    {
        private readonly ApplicationDbContext _context;

        public RolesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task IRolesRepository.AddAsync(Role role, CancellationToken cancellationToken)
        {
            _context.Roles.Add(role);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task IRolesRepository.UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<bool> IRolesRepository.ExistsByNameAsync(string name, CancellationToken cancellationToken)
        {
            return _context.Roles.AnyAsync(x => x.Name == name, cancellationToken);
        }

        Task<List<Role>> IRolesRepository.GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.Roles
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        Task<Role?> IRolesRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Roles
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}