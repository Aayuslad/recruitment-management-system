using MediatR;

using Server.Core.Results;

namespace Server.Application.JobApplications.Commands
{
    public class CreateJobApplicationCommand : IRequest<Result>
    {
        public Guid CandidateId { get; set; }
        public Guid JobOpeningId { get; set; }
    }
}