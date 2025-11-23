using MediatR;

using Server.Application.Aggregates.JobOpenings.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.JobOpenings.Queries
{
    public class GetJobOpeningForRecruiterQuery : IRequest<Result<JobOpeningDetailDTO>>
    {
        public GetJobOpeningForRecruiterQuery(Guid id)
        {
            JobOpeningId = id;
        }

        public Guid JobOpeningId { get; set; }
    }
}