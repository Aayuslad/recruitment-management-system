using MediatR;

using Server.Application.Aggregates.JobApplications.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.JobApplications.Queries
{
    public class GetJobOpeningApplicationsQuery : IRequest<Result<List<JobOpeningApplicationSummaryDTO>>>
    {
        public Guid JobOpeningId { get; set; }

        public GetJobOpeningApplicationsQuery(Guid jobOpeningId)
        {
            JobOpeningId = jobOpeningId;
        }
    }
}