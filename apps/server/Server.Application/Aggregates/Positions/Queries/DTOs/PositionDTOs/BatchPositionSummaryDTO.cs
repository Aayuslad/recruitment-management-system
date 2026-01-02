using Server.Domain.Enums;

namespace Server.Application.Aggregates.Positions.Queries.DTOs.PositionDTOs
{
    public class BatchPositionSummaryDTO
    {
        public Guid BatchId { get; set; }
        public Guid PositionId { get; set; }
        public PositionStatus Status { get; set; }
        public Guid? ClosedByCandidateId { get; set; }
        public string? ClosedByCandidateFullName { get; set; }
        public string? ClosureReason { get; set; }
    }
}