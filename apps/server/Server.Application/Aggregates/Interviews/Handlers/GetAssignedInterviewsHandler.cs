
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Interviews.Queries;
using Server.Application.Aggregates.Interviews.Queries.DTOs;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Interviews.Handlers
{
    internal class GetAssignedInterviewsHandler : IRequestHandler<GetAssignedInterviewsQuery, Result<List<InterviewSummaryDTO>>>
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAssignedInterviewsHandler(IInterviewRepository interviewRepository, IHttpContextAccessor httpContextAccessor)
        {
            _interviewRepository = interviewRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<List<InterviewSummaryDTO>>> Handle(GetAssignedInterviewsQuery request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            if (userIdString is null)
            {
                throw new UnAuthorisedException();
            }

            // step 1: fetch the interviw
            var interviews = await _interviewRepository.GetAllAsync(cancellationToken);

            // step 2: filter the interviews assigned to the interviewer-user
            var assignedInterviews = interviews
                .Where(interview => interview.Participants
                    .Any(participant => participant.UserId == Guid.Parse(userIdString)))
                .ToList();

            // step 3: map dto
            var interviewsDto = new List<InterviewSummaryDTO>();
            foreach (var interview in assignedInterviews)
            {
                var interviewDto = new InterviewSummaryDTO
                {
                    Id = interview.Id,
                    CandidateId = interview.JobApplication.CandidateId,
                    CandidateName = interview.JobApplication.Candidate.FirstName + " " + interview.JobApplication.Candidate.MiddleName + " " + interview.JobApplication.Candidate.LastName,
                    // TODO: this is hilarious, do something for the multiple navigation levels
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

            // step 4: return result
            return Result<List<InterviewSummaryDTO>>.Success(interviewsDto);
        }
    }
}