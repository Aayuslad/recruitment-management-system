using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Notifications.Commands;
using Server.Core.Results;

namespace Server.Application.Notifications.Handlers
{
    internal class MarkNotificationsAsReadHandler : IRequestHandler<MarkNotificationsAsReadCommand, Result>
    {
        private readonly INotificationRepository _notificationRepository;

        public MarkNotificationsAsReadHandler(INotificationRepository rolesRepository)
        {
            _notificationRepository = rolesRepository;
        }

        public async Task<Result> Handle(MarkNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch notifications
            var notificationIds = request.Notifications.Select(x => x.Id);
            var notifications = await _notificationRepository.GetRangeByIdAsync(notificationIds, cancellationToken);

            // step 2: update all as read
            foreach (var notification in notifications)
            {
                notification.MarkAsRead();
            }

            // step 3: persist changes
            await _notificationRepository.UpdateRangeAsync(notifications, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}