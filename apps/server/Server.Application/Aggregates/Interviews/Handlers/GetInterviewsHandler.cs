
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Interviews.Queries;
using Server.Application.Aggregates.Interviews.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Interviews.Handlers
{
    internal class GetinterviewsHandler : IRequestHandler<GetinterviewsQuery, Result<List<InterviewSummaryDTO>>>
    {
        private readonly IInterviewRespository _interviewRespository;

        public GetinterviewsHandler(IInterviewRespository interviewRespository)
        {
            _interviewRespository = interviewRespository;
        }

        public async Task<Result<List<InterviewSummaryDTO>>> Handle(GetinterviewsQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch the interviw
            var interviews = await _interviewRespository.GetAllAsync(cancellationToken);

            // step 2: map dto
            var interviewsDto = new List<InterviewSummaryDTO>();
            foreach (var interview in interviews)
            {
                var interviewDto = new InterviewSummaryDTO
                {
                    Id = interview.Id,
                    CandidateId = interview.JobApplication.CandidateId,
                    CandidateName = interview.JobApplication.Candidate.FirstName + " " + interview.JobApplication.Candidate.MiddleName + " " + interview.JobApplication.Candidate.LastName,
                    // TODO: this is hilarious, do something
                    DesignationId = interview.JobApplication.JobOpening.PositionBatch.DesignationId,
                    DesignationName = interview.JobApplication.JobOpening.PositionBatch.Designation.Name,
                    RoundNumber = interview.RoundNumber,
                    InterviewType = interview.InterviewType,
                    ScheduledAt = interview.ScheduledAt,
                    DurationInMinutes = interview.DurationInMinutes,
                    Status = interview.Status,
                };

                interviewsDto.Add(interviewDto);
            }

            // step 3: return result
            return Result<List<InterviewSummaryDTO>>.Success(interviewsDto);
        }
    }
}