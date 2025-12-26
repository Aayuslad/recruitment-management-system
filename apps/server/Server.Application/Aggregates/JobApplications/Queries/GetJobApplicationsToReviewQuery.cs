using MediatR;

using Server.Application.Aggregates.JobApplications.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.JobApplications.Queries
{
    public class GetJobApplicationsToReviewQuery : IRequest<Result<List<JobApplicationSummaryDTO>>>
    {
        // add paginatin later
    }
}