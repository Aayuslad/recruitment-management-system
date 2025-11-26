using MediatR;

using Server.Application.Aggregates.JobOpenings.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.JobOpenings.Queries
{
    public class GetJobOpeningsForRecruiterQuery : IRequest<Result<List<JobOpeningsDetailDTO>>>
    {
        // add pagination latter
    }
}