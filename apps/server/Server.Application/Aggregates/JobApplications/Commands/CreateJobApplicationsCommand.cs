using MediatR;

using Server.Application.Aggregates.JobApplications.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.JobApplications.Commands
{
    public class CreateJobApplicationsCommand : IRequest<Result>
    {
        public List<JobApplicationDTO> Applications { get; set; } = new List<JobApplicationDTO>();
    }
}