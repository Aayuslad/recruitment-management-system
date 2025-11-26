using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.JobApplications.Commands
{
    public class CreateJobApplicationCommand : IRequest<Result>
    {
        public Guid CandidateId { get; set; }
        public Guid JobOpeningId { get; set; }
    }
}