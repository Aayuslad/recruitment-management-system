using MediatR;

using Server.Application.Aggregates.JobApplications.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.JobApplications.Queries
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