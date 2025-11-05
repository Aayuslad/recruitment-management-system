using MediatR;

using Server.Application.JobOpenings.Queries.DTOs.ForCandiateClient;
using Server.Core.Results;

namespace Server.Application.JobOpenings.Queries
{
    public class GetJobOpeningsForCandidateQuery : IRequest<Result<List<JobOpeningsDetailDTO>>>
    {
        // add pagination latter
    }
}