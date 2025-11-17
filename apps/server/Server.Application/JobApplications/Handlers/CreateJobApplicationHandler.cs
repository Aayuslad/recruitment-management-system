using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.JobApplications.Commands;
using Server.Core.Results;
using Server.Domain.Entities;

namespace Server.Application.JobApplications.Handlers
{
    internal class CreateJobApplicationHandler : IRequestHandler<CreateJobApplicationCommand, Result>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public CreateJobApplicationHandler(IJobApplicationRepository jobApplicationRepository, IHttpContextAccessor contextAccessor)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: check if alredy exists
            var result = await _jobApplicationRepository.ExistsByCandidateAndOpeningAsync(request.JobOpeningId, request.CandidateId, cancellationToken);
            if (result)
            {
                return Result.Failure("Candidate is alredy applied in this job application", 409);
            }

            // step 2: create entity
            var application = JobApplication.Create(
                    id: null,
                    createdBy: Guid.Parse(userIdString),
                    candidateId: request.CandidateId,
                    jobOpeningId: request.JobOpeningId
                );

            // step 3: persist entity
            await _jobApplicationRepository.AddAsync(application, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}
