using MediatR;

using Server.Application.Aggregates.Interviews.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Interviews.Queries
{
    public class GetAssignedInterviewsQuery : IRequest<Result<List<InterviewSummaryDTO>>>
    {
    }
}