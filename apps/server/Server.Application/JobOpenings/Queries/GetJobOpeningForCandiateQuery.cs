using MediatR;

using Server.Application.JobOpenings.Queries.DTOs.ForCandiateClient;
using Server.Core.Results;

namespace Server.Application.JobOpenings.Queries
{
    public class GetJobOpeningForCandiateQuery : IRequest<Result<JobOpeningDetailDTO>>
    {
        public GetJobOpeningForCandiateQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}