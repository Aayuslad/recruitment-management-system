using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Notifications.Queries;
using Server.Application.Aggregates.Notifications.Queries.DTOs;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Notifications.Handlers
{
    internal class GetUserNotificationsHandler : IRequestHandler<GetUserNotificationsQuery, Result<List<NotificationDetailDTO>>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserNotificationsHandler(INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _notificationRepository = notificationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<List<NotificationDetailDTO>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedException();
            }

            // step 1: fetch all notifications of user
            var notifications = await _notificationRepository.GetAllForUserAsync(Guid.Parse(userIdString), cancellationToken);

            // step 2: list dtos
            var notificatinoDtos = notifications.Select(
                selector: x => new NotificationDetailDTO
                {
                    Id = x.Id,
                    FromUserId = x.FromUserId,
                    FromUserName = x.FromUser.Auth.UserName,
                    Subject = x.Subject,
                    Message = x.Message,
                    IsRead = x.IsRead,
                }
            ).ToList();

            // step 3: return result
            return Result<List<NotificationDetailDTO>>.Success(notificatinoDtos);
        }
    }
}