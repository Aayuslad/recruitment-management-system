using MediatR;

using Server.Application.Aggregates.Positions.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Positions.Commands
{
    public class CreatePositionBatchCommand : IRequest<Result>
    {
        public int NumberOfPositions { get; set; }
        public string? Description { get; set; }
        public Guid DesignationId { get; set; }
        public string JobLocation { get; set; } = string.Empty;
        public float MinCTC { get; set; } = default;
        public float MaxCTC { get; set; } = default;
        public List<PositionReviewersDTO>? Reviewers { get; set; } = null!;
        public List<PositionSkillOverRideDTO>? SkillOverRides { get; set; }
    }
}