using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.JobApplications.Queries;
using Server.Application.JobApplications.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.JobApplications.Handlers
{
    internal class GetJobApplicationsHandler : IRequestHandler<GetJobApplicationsQuery, Result<List<JobApplicationSummaryDTO>>>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public GetJobApplicationsHandler(IJobApplicationRepository jobApplicationRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
        }

        public async Task<Result<List<JobApplicationSummaryDTO>>> Handle(GetJobApplicationsQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch applications
            var applications = await _jobApplicationRepository.GetAllAsync(cancellationToken);

            // step 2: list Dto
            var applicationDtos = new List<JobApplicationSummaryDTO>();

            foreach (var application in applications)
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

            // step 3: return result
            return Result<List<JobApplicationSummaryDTO>>.Success(applicationDtos);
        }
    }
}