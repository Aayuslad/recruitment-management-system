using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Exeptions;
using Server.Application.JobApplications.Commands;
using Server.Application.JobApplications.Commands.DTOs;
using Server.Core.Results;
using Server.Domain.Entities;

namespace Server.Application.JobApplications.Handlers
{
    internal class CreateJobApplicationsHandler : IRequestHandler<CreateJobApplicationsCommand, Result>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public CreateJobApplicationsHandler(IJobApplicationRepository jobApplicationRepository, IHttpContextAccessor contextAccessor)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(CreateJobApplicationsCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedExeption();
            }

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
                        createdBy: Guid.Parse(userIdString),
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