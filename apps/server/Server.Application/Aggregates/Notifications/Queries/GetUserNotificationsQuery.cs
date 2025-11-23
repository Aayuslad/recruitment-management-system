using MediatR;

using Server.Application.Aggregates.Notifications.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Notifications.Queries
{
    public class GetUserNotificationsQuery : IRequest<Result<List<NotificationDetailDTO>>>
    {
    }
}