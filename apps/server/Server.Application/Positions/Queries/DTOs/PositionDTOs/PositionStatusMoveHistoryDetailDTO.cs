using Server.Domain.Enums;

namespace Server.Application.Positions.Queries.DTOs.PositionDTOs
{
    public class PositionStatusMoveHistoryDetailDTO
    {
        public Guid PositionId { get; set; }
        public PositionStatus MovedTo { get; set; }
        public string? Comments { get; set; }
        public DateTime MovedAt { get; set; }
        public Guid? MovedById { get; set; }
        public string MovedByUserName { get; set; } = null!;
    }
}