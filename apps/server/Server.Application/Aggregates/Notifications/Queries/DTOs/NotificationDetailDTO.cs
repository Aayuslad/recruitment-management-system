namespace Server.Application.Aggregates.Notifications.Queries.DTOs
{
    public class NotificationDetailDTO
    {
        public Guid Id { get; set; }
        public Guid? FromUserId { get; set; }
        public string? FromUserName { get; set; }
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; }
    }
}