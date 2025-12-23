using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities.Employees;
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

        public Task AddAsync(Employee employee, CancellationToken cancellationToken)
        {
            _context.Employees.Add(employee);
            return _context.SaveChangesAsync(cancellationToken);
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