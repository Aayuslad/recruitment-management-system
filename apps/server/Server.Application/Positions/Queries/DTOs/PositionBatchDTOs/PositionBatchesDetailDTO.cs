namespace Server.Application.Positions.Queries.DTOs.PositionBatchDTOs
{
    public class PositionBatchesDetailDTO
    {
        public List<PositionBatchSummaryDTO> Batches { get; set; } = new List<PositionBatchSummaryDTO>();
    }
}