using MediatR;

using Server.Application.Aggregates.JobOpenings.Commands.DTOs;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobOpenings.Commands
{
    public class CreateJobOpeningCommand : IRequest<Result>
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public JobOpeningType Type { get; set; }
        public Guid PositionBatchId { get; set; }
        public List<JobOpeningInterviewerDTO> Interviewers { get; set; } =
            new List<JobOpeningInterviewerDTO>();
        public List<InterviewRoundTemplateDTO> InterviewRounds { get; set; } =
            new List<InterviewRoundTemplateDTO>();
        public List<SkillOverRideDTO> SkillOverRides { get; set; } =
            new List<SkillOverRideDTO>();
    }
}