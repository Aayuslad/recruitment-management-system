using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.JobApplications.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities.JobApplications;

namespace Server.Application.Aggregates.JobApplications.Handlers
{
    internal class CreateJobApplicationHandler : IRequestHandler<CreateJobApplicationCommand, Result>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IUserContext _userContext;

        public CreateJobApplicationHandler(IJobApplicationRepository jobApplicationRepository, IUserContext userContext)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
        {
            // step 1: check if alredy exists
            var result = await _jobApplicationRepository.ExistsByCandidateAndOpeningAsync(request.JobOpeningId, request.CandidateId, cancellationToken);
            if (result)
            {
                throw new ConflictException("Job application already exists for this candidate and job opening.");
            }

            // step 2: create entity
            var application = JobApplication.Create(
                    id: null,
                    createdBy: _userContext.UserId,
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