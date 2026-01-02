using MediatR;

using Server.Application.Aggregates.JobApplications.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.JobApplications.Queries
{
    public class GetJobApplicationsQuery : IRequest<Result<List<JobApplicationSummaryDTO>>>
    {
        // add paginatin later
    }
}