using MediatR;

using Server.Application.Interviews.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Interviews.Queries
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