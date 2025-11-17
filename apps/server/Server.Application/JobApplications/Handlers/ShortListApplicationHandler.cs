using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.JobApplications.Commands;
using Server.Core.Results;

namespace Server.Application.JobApplications.Handlers
{
    internal class ShortListApplicationHandler : IRequestHandler<ShortListApplicationCommand, Result>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public ShortListApplicationHandler(IJobApplicationRepository jobApplicationRepository, IHttpContextAccessor contextAccessor)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(ShortListApplicationCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: check if exists
            var application = await _jobApplicationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (application is null)
            {
                return Result.Failure("Job Application does not exists", 409);
            }

            // step 2: move to shortlist stage
            application.Shortlist(Guid.Parse(userIdString));

            // step 3: persist entity
            await _jobApplicationRepository.UpdateAsync(application, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}
