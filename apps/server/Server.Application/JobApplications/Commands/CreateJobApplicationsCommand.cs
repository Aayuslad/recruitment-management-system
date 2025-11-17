using MediatR;

using Server.Application.JobApplications.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.JobApplications.Commands
{
    public class CreateJobApplicationsCommand : IRequest<Result>
    {
        public List<JobApplicationDTO> Applications { get; set; } = new List<JobApplicationDTO>();
    }
}
