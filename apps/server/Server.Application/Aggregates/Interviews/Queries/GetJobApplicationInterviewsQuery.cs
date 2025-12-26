using MediatR;

using Server.Application.Aggregates.Interviews.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Interviews.Queries
{
    public class GetJobApplicationInterviewsQuery : IRequest<Result<List<InterviewSummaryForApplicationDTO>>>
    {
        public Guid JobApplicationId { get; set; }

        public GetJobApplicationInterviewsQuery(Guid jobApplicationId)
        {
            JobApplicationId = jobApplicationId;
        }
    }
}