using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task<List<Employee>> IEmployeeRepository.GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.Employees
                .AsNoTracking()
                .Include(x => x.Designation)
                .ToListAsync(cancellationToken);
        }
    }
}