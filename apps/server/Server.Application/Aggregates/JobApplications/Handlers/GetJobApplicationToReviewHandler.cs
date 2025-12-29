using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.JobApplications.Queries;
using Server.Application.Aggregates.JobApplications.Queries.DTOs;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.JobApplications.Handlers
{
    internal class GetJobApplicationsToReviewHandler : IRequestHandler<GetJobApplicationsToReviewQuery, Result<List<JobApplicationSummaryDTO>>>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetJobApplicationsToReviewHandler(IJobApplicationRepository jobApplicationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<List<JobApplicationSummaryDTO>>> Handle(GetJobApplicationsToReviewQuery request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedException();
            }

            // step 1: fetch applications
            var applications = await _jobApplicationRepository.GetAllAsync(cancellationToken);

            // step 2: filter applications assigned to this reviewer-user
            var assignedApplications = applications
                .Where(app => app.JobOpening.PositionBatch.Reviewers.Any(r => r.ReviewerId.ToString() == userIdString))
                .ToList();

            // step 3: list Dto
            var applicationDtos = new List<JobApplicationSummaryDTO>();

            foreach (var application in assignedApplications)
            {
                var summaryDto = new JobApplicationSummaryDTO
                {
                    Id = application.Id,
                    CandidateId = application.CandidateId,
                    CandidateName = $"{application.Candidate.FirstName} {application.Candidate.MiddleName} {application.Candidate.LastName}",
                    JobOpeningId = application.JobOpeningId,
                    Designation = application.JobOpening.PositionBatch.Designation.Name,
                    AppliedAt = application.AppliedAt,
                    Status = application.Status,
                    AvgRating = application.Feedbacks.Count != 0 ? application.Feedbacks.Average(f => f.Rating) : null
                };

                applicationDtos.Add(summaryDto);
            }

            // step 4: return result
            return Result<List<JobApplicationSummaryDTO>>.Success(applicationDtos);
        }
    }
}