using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.JobApplications.Queries;
using Server.Application.Aggregates.JobApplications.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.JobApplications.Handlers
{
    internal class GetJobOpeningApplicationsHandler : IRequestHandler<GetJobOpeningApplicationsQuery, Result<List<JobOpeningApplicationSummaryDTO>>>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public GetJobOpeningApplicationsHandler(IJobApplicationRepository jobApplicationRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
        }

        public async Task<Result<List<JobOpeningApplicationSummaryDTO>>> Handle(GetJobOpeningApplicationsQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch applications
            var applications = await _jobApplicationRepository.GetAllByJobOpeningIdAsync(request.JobOpeningId, cancellationToken);

            // step 2: list Dto
            var applicationDtos = new List<JobOpeningApplicationSummaryDTO>();

            foreach (var application in applications)
            {
                var summaryDto = new JobOpeningApplicationSummaryDTO
                {
                    Id = application.Id,
                    CandidateId = application.CandidateId,
                    CandidateName = $"{application.Candidate.FirstName} {application.Candidate.MiddleName} {application.Candidate.LastName}",
                    AppliedAt = application.AppliedAt,
                    Status = application.Status,
                };

                applicationDtos.Add(summaryDto);
            }

            // step 3: return result
            return Result<List<JobOpeningApplicationSummaryDTO>>.Success(applicationDtos);
        }
    }
}