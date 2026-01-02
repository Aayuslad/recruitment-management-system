
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Interviews.Queries;
using Server.Application.Aggregates.Interviews.Queries.DTOs;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Interviews.Handlers
{
    internal class GetInterviewHandler : IRequestHandler<GetInterviewQuery, Result<InterviewDetailDTO>>
    {
        private readonly IInterviewRepository _interviewRepository;

        public GetInterviewHandler(IInterviewRepository interviewRepository)
        {
            _interviewRepository = interviewRepository;
        }

        public async Task<Result<InterviewDetailDTO>> Handle(GetInterviewQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch the interviw
            var interview = await _interviewRepository.GetByIdAsync(request.Id, cancellationToken);
            if (interview is null)
            {
                throw new NotFoundException($"Interview Not Found.");
            }

            // step 2: map dto
            var interviewDto = new InterviewDetailDTO
            {
                Id = interview.Id,
                JobApplicationId = interview.JobApplicationId,
                CandidateId = interview.JobApplication.CandidateId,
                CandidateName = interview.JobApplication.Candidate.FirstName + " " + interview.JobApplication.Candidate.MiddleName + " " + interview.JobApplication.Candidate.LastName,
                DesignationId = interview.JobApplication.JobOpening.PositionBatch.DesignationId,
                DesignationName = interview.JobApplication.JobOpening.PositionBatch.Designation.Name,
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