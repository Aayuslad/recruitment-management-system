using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Exeptions;
using Server.Application.JobApplications.Commands;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.JobApplications.Handlers
{
    internal class MoveJobApplicationStatusHandler : IRequestHandler<MoveJobApplicationStatusCommand, Result>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public MoveJobApplicationStatusHandler(IJobApplicationRepository jobApplicationRepository, IHttpContextAccessor contextAccessor)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(MoveJobApplicationStatusCommand request, CancellationToken cancellationToken)
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

            // step 2: move to shortlist stage
            application.MoveStatus(Guid.Parse(userIdString), request.MoveTo);

            // step 3: persist entity
            await _jobApplicationRepository.UpdateAsync(application, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}