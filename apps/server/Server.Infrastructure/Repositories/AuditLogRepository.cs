using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    public class AuditLogRepository: IAuditLogRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task IAuditLogRepository.AddAsync(AuditLog auditLog, CancellationToken cancellationToken)
        {
            _context.AuditLogs.Add(auditLog);
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
