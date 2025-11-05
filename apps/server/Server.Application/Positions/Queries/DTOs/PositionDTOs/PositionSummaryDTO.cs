using Server.Domain.Enums;

namespace Server.Application.Positions.Queries.DTOs.PositionDTOs
{
    public class PositionSummaryDTO
    {
        public Guid BatchId { get; set; }
        public Guid PositionId { get; set; }
        public string? Descripcion { get; set; }
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; } = null!;
        public string JobLocation { get; set; } = null!;
        public float MinCTC { get; set; } = default;
        public float MaxCTC { get; set; } = default;
        public PositionStatus Status { get; set; }
        public Guid? ClosedByCandidate { get; set; }
        public string? ClosureReason { get; set; }
    }
}