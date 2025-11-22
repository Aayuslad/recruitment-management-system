using MediatR;

using Server.Application.Notifications.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.Notifications.Commands
{
    public class MarkNotificationsAsReadCommand : IRequest<Result>
    {
        public List<NotificationDTO> Notifications { get; set; } = new List<NotificationDTO>();
    }
}