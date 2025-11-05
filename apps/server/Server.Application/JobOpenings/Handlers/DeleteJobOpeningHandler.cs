
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.JobOpenings.Commands;
using Server.Core.Results;

namespace Server.Application.JobOpenings.Handlers
{
    internal class DeleteJobOpeningHandler : IRequestHandler<DeleteJobOpeningCommand, Result>
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteJobOpeningHandler(IJobOpeningRepository jobOpeningRepository, IHttpContextAccessor httpContext)
        {
            _jobOpeningRepository = jobOpeningRepository;
            _httpContextAccessor = httpContext;
        }

        public async Task<Result> Handle(DeleteJobOpeningCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: find the position
            var jobOpening = await _jobOpeningRepository.GetByIdAsync(request.JobOpeningId, cancellationToken);
            if (jobOpening == null)
            {
                return Result.Failure("Job opening not found", 404);
            }

            // step 2: soft delet
            jobOpening.Delete(Guid.Parse(userIdString));

            // step 3: persist entity
            await _jobOpeningRepository.UpdateAysnc(jobOpening, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}