
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Candidates.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Handlers
{
    internal class DeleteCandidateHandler : IRequestHandler<DeleteCandidateCommand, Result>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUserContext _userContext;


        public DeleteCandidateHandler(ICandidateRepository candidateRepository, IUserContext userContext)
        {
            _candidateRepository = candidateRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(DeleteCandidateCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch the entity
            var candidate = await _candidateRepository.GetByIdAsync(request.Id, cancellationToken);
            if (candidate == null)
            {
                throw new NotFoundException("Candidate not found.");
            }

            // step 2: soft delete
            candidate.Delete(_userContext.UserId);

            // step 3: persist
            await _candidateRepository.UpdateAsync(candidate, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}