using MediatR;

using Server.Application.Aggregates.Interviews.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Interviews.Queries
{
    public class GetInterviewQuery : IRequest<Result<InterviewDetailDTO>>
    {
        public GetInterviewQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}