using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Exeptions;
using Server.Application.JobApplications.Commands;
using Server.Core.Results;

namespace Server.Application.JobApplications.Handlers
{
    internal class DeleteJobApplicationHandler : IRequestHandler<DeleteJobApplicationCommand, Result>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public DeleteJobApplicationHandler(IJobApplicationRepository jobApplicationRepository, IHttpContextAccessor contextAccessor)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(DeleteJobApplicationCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedExeption();
            }

            // step 1: check if exists
            var application = await _jobApplicationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (application is null)
            {
                throw new NotFoundExeption("Job Application Not Found.");
            }

            // step 2: soft delete
            application.Delete(Guid.Parse(userIdString));

            // step 3: persist
            await _jobApplicationRepository.UpdateAsync(application, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}