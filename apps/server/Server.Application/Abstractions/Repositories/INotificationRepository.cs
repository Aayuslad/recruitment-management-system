using Server.Domain.Entities.Notifications;

namespace Server.Application.Abstractions.Repositories
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllForUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<Notification>> GetRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
        Task UpdateRangeAsync(IEnumerable<Notification> notifications, CancellationToken cancellationToken);
    }
}