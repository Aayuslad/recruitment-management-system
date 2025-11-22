using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task INotificationRepository.UpdateRangeAsync(IEnumerable<Notification> notifications, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<List<Notification>> INotificationRepository.GetAllForUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return _context.Notifications
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .Include(x => x.FromUser)
                        .ThenInclude(x => x.Auth)
                    .ToListAsync(cancellationToken);
        }

        Task<List<Notification>> INotificationRepository.GetRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            return _context.Notifications
                    .AsTracking()
                    .Where(x => ids.Contains(x.Id))
                    .ToListAsync(cancellationToken);
        }
    }
}