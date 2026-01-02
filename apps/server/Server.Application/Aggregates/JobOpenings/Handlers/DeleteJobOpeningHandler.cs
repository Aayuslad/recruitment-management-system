
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.JobOpenings.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.JobOpenings.Handlers
{
    internal class DeleteJobOpeningHandler : IRequestHandler<DeleteJobOpeningCommand, Result>
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IUserContext _userContext;

        public DeleteJobOpeningHandler(IJobOpeningRepository jobOpeningRepository, IUserContext userContext)
        {
            _jobOpeningRepository = jobOpeningRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(DeleteJobOpeningCommand request, CancellationToken cancellationToken)
        {
            // step 1: find the position
            var jobOpening = await _jobOpeningRepository.GetByIdAsync(request.JobOpeningId, cancellationToken);
            if (jobOpening == null)
            {
                throw new NotFoundException("Job Opening Not Found.");
            }

            // step 2: soft delete
            jobOpening.Delete(_userContext.UserId);

            // step 3: persist entity
            await _jobOpeningRepository.UpdateAysnc(jobOpening, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}