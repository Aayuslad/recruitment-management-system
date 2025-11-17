using MediatR;

using Server.Application.JobApplications.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.JobApplications.Queries
{
    public class GetJobApplicationsQuery : IRequest<Result<List<JobApplicationSummaryDTO>>>
    {
        // add paginatin later
    }
}