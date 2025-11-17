using MediatR;

using Server.Application.JobApplications.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.JobApplications.Queries
{
    public class GetJobApplicationQuery : IRequest<Result<JobApplicationDetailDTO>>
    {
        public GetJobApplicationQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
 