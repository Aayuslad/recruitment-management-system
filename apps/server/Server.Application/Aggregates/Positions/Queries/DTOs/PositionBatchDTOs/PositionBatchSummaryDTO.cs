namespace Server.Application.Aggregates.Positions.Queries.DTOs.PositionBatchDTOs
{
    public class PositionBatchSummaryDTO
    {
        public Guid BatchId { get; set; }
        public string? Description { get; set; }
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; } = null!;
        public string JobLocation { get; set; } = null!;
        public float MinCTC { get; set; }
        public float MaxCTC { get; set; }
        public int PositionsCount { get; set; }
        public int ClosedPositionsCount { get; set; }
        public int PositionsOnHoldCount { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? CreatedByUserName { get; set; } = null!;
    }
}