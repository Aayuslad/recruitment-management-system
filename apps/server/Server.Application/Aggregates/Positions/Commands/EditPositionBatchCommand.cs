using MediatR;

using Server.Application.Aggregates.Positions.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Positions.Commands
{
    public class EditPositionBatchCommand : IRequest<Result>
    {
        public Guid PositionBatchId { get; set; }
        public string? Description { get; set; }
        public Guid DesignationId { get; set; }
        public string JobLocation { get; set; } = null!;
        public float MinCTC { get; set; }
        public float MaxCTC { get; set; }
        public List<PositionReviewersDTO> Reviewers { get; set; } =
            new List<PositionReviewersDTO>();
        public List<PositionSkillOverRideDTO> SkillOverRides { get; set; } =
            new List<PositionSkillOverRideDTO>();
    }
}