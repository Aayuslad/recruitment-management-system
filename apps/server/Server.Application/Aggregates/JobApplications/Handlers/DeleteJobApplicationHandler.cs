using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.JobApplications.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.JobApplications.Handlers
{
    internal class DeleteJobApplicationHandler : IRequestHandler<DeleteJobApplicationCommand, Result>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IUserContext _userContext;

        public DeleteJobApplicationHandler(IJobApplicationRepository jobApplicationRepository, IUserContext userContext)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(DeleteJobApplicationCommand request, CancellationToken cancellationToken)
        {
            // step 1: check if exists
            var application = await _jobApplicationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (application is null)
            {
                throw new NotFoundException("Job Application Not Found.");
            }

            // step 2: soft delete
            application.Delete(_userContext.UserId);

            // step 3: persist
            await _jobApplicationRepository.UpdateAsync(application, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}