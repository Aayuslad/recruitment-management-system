using Server.Domain.Enums;

namespace Server.Application.Aggregates.Positions.Queries.DTOs.PositionDTOs
{
    public class PositionDetailDTO
    {
        public Guid BatchId { get; set; }
        public Guid PositionId { get; set; }
        public string? Descripcion { get; set; }
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; } = null!;
        public string JobLocation { get; set; } = null!;
        public float MinCTC { get; set; }
        public float MaxCTC { get; set; }
        public PositionStatus Status { get; set; }
        public Guid? ClosedByCandidate { get; set; }
        public string? ClosureReason { get; set; }
        public List<SkillDetailDTO> Skills { get; set; } = new List<SkillDetailDTO>();
        public List<ReviewersDetailDTO> Reviewers { get; set; } = new List<ReviewersDetailDTO>();
        public List<PositionStatusMoveHistoryDetailDTO> MoveHistory { get; set; } = new List<PositionStatusMoveHistoryDetailDTO>();
    }
}