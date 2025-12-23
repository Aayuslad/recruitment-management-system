
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.JobApplications.Queries;
using Server.Application.Aggregates.JobApplications.Queries.DTOs;
using Server.Application.Exeptions;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobApplications.Handlers
{
    internal class GetJobApplicationHandler : IRequestHandler<GetJobApplicationQuery, Result<JobApplicationDetailDTO>>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public GetJobApplicationHandler(IJobApplicationRepository jobApplicationRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
        }

        public async Task<Result<JobApplicationDetailDTO>> Handle(GetJobApplicationQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch job application
            var application = await _jobApplicationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (application is null)
            {
                throw new NotFoundExeption("Job Application Not Found.");
            }

            // step 2: create dto
            var applicationDTO = new JobApplicationDetailDTO
            {
                Id = application.Id,
                CandidateId = application.CandidateId,
                CandidateName = $"{application.Candidate.FirstName} {application.Candidate.MiddleName} {application.Candidate.LastName}",
                JobOpeningId = application.JobOpeningId,
                Designation = application.JobOpening.PositionBatch.Designation.Name,
                AppliedAt = application.AppliedAt,
                Status = application.Status,
                JobApplicationFeedbacks = application.Feedbacks.Where(x => x.Stage == FeedbackStage.Review).Select(
                        selector: x => new FeedbackDetailDTO
                        {
                            Id = x.Id,
                            GivenById = x.GivenById,
                            GivenByName = x.GivenByUser.Auth.UserName,
                            Comment = x.Comment,
                            Rating = x.Rating,
                            SkillFeedbacks = x.SkillFeedbacks.Select(
                                    selector: x => new SkillFeedbackDetailDTO
                                    {
                                        SkillId = x.SkillId,
                                        SkillName = x.Skill.Name,
                                        Rating = x.Rating,
                                        AssessedExpYears = x.AssessedExpYears,
                                    }
                                ).ToList(),
                        }
                    ).ToList(),
                InterviewFeedbacks = application.Feedbacks.Where(x => x.Stage == FeedbackStage.Interview).Select(
                        selector: x => new FeedbackDetailDTO
                        {
                            Id = x.Id,
                            GivenById = x.GivenById,
                            GivenByName = x.GivenByUser.Auth.UserName,
                            Comment = x.Comment,
                            Rating = x.Rating,
                            SkillFeedbacks = x.SkillFeedbacks.Select(
                                    selector: x => new SkillFeedbackDetailDTO
                                    {
                                        SkillId = x.SkillId,
                                        SkillName = x.Skill.Name,
                                        Rating = x.Rating,
                                        AssessedExpYears = x.AssessedExpYears,
                                    }
                                ).ToList(),
                        }
                    ).ToList(),
                StatusMoveHistories = application.StatusMoveHistories.Select(
                        selector: x => new StatusMoveHistoryDetailDTO
                        {
                            Id = x.Id,
                            StatusMovedTo = x.StatusMovedTo,
                            MovedById = x.MovedById,
                            MovedByName = x.MovedByUser?.Auth.UserName ?? "System",
                            MovedAt = x.MovedAt,
                            Comment = x.Comment,
                        }
                    ).ToList(),
                AvgRating = application.Feedbacks.Count != 0 ? application.Feedbacks.Average(f => f.Rating) : null
            };

            // step 3: return result
            return Result<JobApplicationDetailDTO>.Success(applicationDTO);
        }
    }
}