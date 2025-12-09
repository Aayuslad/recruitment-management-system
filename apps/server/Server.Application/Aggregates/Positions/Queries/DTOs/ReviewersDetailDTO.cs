namespace Server.Application.Aggregates.Positions.Queries.DTOs
{
    public class ReviewersDetailDTO
    {
        public Guid ReviewerUserId { get; set; }
        public string ReviewerUserName { get; set; } = null!;
        public string ReviewerUserEmail { get; set; } = null!;
    }
}