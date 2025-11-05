using Server.Application.Positions.Queries.DTOs.PositionDTOs;

namespace Server.Application.Positions.Queries.DTOs
{
    public class PositionsDetailDTO
    {
        public List<PositionSummaryDTO> Positions { get; set; } = new List<PositionSummaryDTO>();
    }
}