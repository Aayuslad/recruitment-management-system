using MediatR;

using Server.Application.Notifications.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Notifications.Queries
{
    public class GetUserNotificationsQuery : IRequest<Result<List<NotificationDetailDTO>>>
    {
    }
}