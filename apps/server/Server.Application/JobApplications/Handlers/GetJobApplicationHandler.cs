
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.JobApplications.Queries;
using Server.Application.JobApplications.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.JobApplications.Handlers
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
                return Result<JobApplicationDetailDTO>.Failure("Job Application does not exists", 404);
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
                Feedbacks = application.Feedbacks.Select(
                        selector: x => new FeedbackDetailDTO
                        {
                            Id = x.Id,
                            GivenById = x.GivenById,
                            GivenByName = x.GivenByUser.Auth.UserName,
                            Stage = x.Stage,
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
                            MovedByName = x.MovedByUser.Auth.UserName,
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