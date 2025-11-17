using MediatR;

using Server.Application.JobOpenings.Queries.DTOs.ForRecruterClient;
using Server.Core.Results; 

namespace Server.Application.JobOpenings.Queries
{
    public class GetJobOpeningsForRecruiterQuery : IRequest<Result<List<JobOpeningsDetailDTO>>>
    {
        // add pagination latter
    }
}