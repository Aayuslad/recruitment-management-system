using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.JobApplications.Commands;
using Server.Application.Aggregates.JobApplications.Commands.DTOs;
using Server.Core.Results;
using Server.Domain.Entities.JobApplications;

namespace Server.Application.Aggregates.JobApplications.Handlers
{
    internal class CreateJobApplicationsHandler : IRequestHandler<CreateJobApplicationsCommand, Result>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IUserContext _userContext;

        public CreateJobApplicationsHandler(IJobApplicationRepository jobApplicationRepository, IUserContext userContext)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(CreateJobApplicationsCommand request, CancellationToken cancellationToken)
        {
            // step 1: enumurate and check if any already exists
            // TODO: this is not efficient, can be checked in bulk at db level, improve it
            var applicationstoCreate = new List<JobApplicationDTO>();
            foreach (var application in request.Applications)
            {
                var result = await _jobApplicationRepository.ExistsByCandidateAndOpeningAsync(application.JobOpeningId, application.CandidateId, cancellationToken);
                if (!result)
                {
                    applicationstoCreate.Add(application);
                }
            }

            // step 2: create entities
            var applications = new List<JobApplication>();
            foreach (var applicationDto in applicationstoCreate)
            {
                var application = JobApplication.Create(
                        id: null,
                        createdBy: _userContext.UserId,
                        candidateId: applicationDto.CandidateId,
                        jobOpeningId: applicationDto.JobOpeningId
                    );

                applications.Add(application);
            }

            // step 3: persist entity
            await _jobApplicationRepository.AddRangeAsync(applications, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}