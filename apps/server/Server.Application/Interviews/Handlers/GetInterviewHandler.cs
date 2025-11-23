
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Exeptions;
using Server.Application.Interviews.Queries;
using Server.Application.Interviews.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Interviews.Handlers
{
    internal class GetInterviewHandler : IRequestHandler<GetInterviewQuery, Result<InterviewDetailDTO>>
    {
        private readonly IInterviewRespository _interviewRespository;

        public GetInterviewHandler(IInterviewRespository interviewRespository)
        {
            _interviewRespository = interviewRespository;
        }

        public async Task<Result<InterviewDetailDTO>> Handle(GetInterviewQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch the interviw
            var interview = await _interviewRespository.GetByIdAsync(request.Id, cancellationToken);
            if (interview is null)
            {
                throw new NotFoundExeption($"Interview Not Found.");
            }

            // step 2: map dto
            var interviewDto = new InterviewDetailDTO
            {
                Id = interview.Id,
                JobApplicationId = interview.JobApplicationId,
                RoundNumber = interview.RoundNumber,
                InterviewType = interview.InterviewType,
                ScheduledAt = interview.ScheduledAt,
                DurationInMinutes = interview.DurationInMinutes,
                MeetingLink = interview.MeetingLink,
                Status = interview.Status,
                Feedbacks = interview.Feedbacks.Select(
                        selector: x => new FeedbackDetailDTO
                        {
                            Id = x.Id,
                            GivenById = x.GivenById,
                            GivenByName = x.GivenByUser.Auth.UserName,
                            Stage = x.Stage,
                            Comment = x.Comment,
                            Rating = x.Rating,
                            SkillFeedbacks = x.SkillFeedbacks.Select(
                                    selector: y => new SkillFeedbackDetailDTO
                                    {
                                        SkillId = y.SkillId,
                                        SkillName = y.Skill.Name,
                                        Rating = y.Rating,
                                        AssessedExpYears = y.AssessedExpYears,
                                    }
                                ).ToList(),
                        }
                    ).ToList(),
                Participants = interview.Participants.Select(
                        selector: x => new InterviewParticipantDetailDTO
                        {
                            Id = x.Id,
                            UserId = x.UserId,
                            ParticipantUserName = x.User.Auth.UserName,
                            Role = x.Role,
                        }
                    ).ToList(),
            };

            // step 3: return result
            return Result<InterviewDetailDTO>.Success(interviewDto);
        }
    }
}