
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Interviews.Queries;
using Server.Application.Aggregates.Interviews.Queries.DTOs;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Interviews.Handlers
{
    internal class GetJobApplicationInterviewsHandler : IRequestHandler<GetJobApplicationInterviewsQuery, Result<List<InterviewSummaryForApplicationDTO>>>
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetJobApplicationInterviewsHandler(IInterviewRepository interviewRepository, IHttpContextAccessor httpContextAccessor)
        {
            _interviewRepository = interviewRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<List<InterviewSummaryForApplicationDTO>>> Handle(GetJobApplicationInterviewsQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch the interviw
            var interviews = await _interviewRepository.GetAllByJobApplicationIdAsync(request.JobApplicationId, cancellationToken);

            // step 2: map dto
            var interviewsDto = new List<InterviewSummaryForApplicationDTO>();
            foreach (var interview in interviews)
            {
                var interviewDto = new InterviewSummaryForApplicationDTO
                {
                    Id = interview.Id,
                    RoundNumber = interview.RoundNumber,
                    InterviewType = interview.InterviewType,
                    ScheduledAt = interview.ScheduledAt,
                    DurationInMinutes = interview.DurationInMinutes,
                    Status = interview.Status,
                    Participants = interview.Participants.Select(
                        selector: x => new InterviewParticipantDetailDTO
                        {
                            Id = x.Id,
                            UserId = x.UserId,
                            ParticipantUserName = x.User.Auth.UserName,
                            Role = x.Role
                        }
                    ).ToList()
                };

                interviewsDto.Add(interviewDto);
            }

            // step 3: return result
            return Result<List<InterviewSummaryForApplicationDTO>>.Success(interviewsDto);
        }
    }
}