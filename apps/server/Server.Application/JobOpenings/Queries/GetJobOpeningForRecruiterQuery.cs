using MediatR;

using Server.Application.JobOpenings.Queries.DTOs.ForRecruterClient;
using Server.Core.Results;

namespace Server.Application.JobOpenings.Queries
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