using Server.Domain.Entities;

namespace Server.Application.Abstractions.Repositories
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLog auditLog, CancellationToken cancellation);
    }
}
