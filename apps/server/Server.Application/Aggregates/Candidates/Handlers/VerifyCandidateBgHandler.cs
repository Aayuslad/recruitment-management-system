
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Candidates.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Handlers
{
    internal class VerifyCandidateBgHandler : IRequestHandler<VerifyCandidateBgCommand, Result>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUserContext _userContext;

        public VerifyCandidateBgHandler(ICandidateRepository candidateRepository, IUserContext userContext)
        {
            _candidateRepository = candidateRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(VerifyCandidateBgCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch the candidate
            var candidate = await _candidateRepository.GetByIdAsync(request.CandidateId, cancellationToken);
            if (candidate == null)
            {
                throw new NotFoundException("Candidate Not Found");
            }

            // step 2: verify
            candidate.MarkBackgroundVerified(_userContext.UserId);

            // step 3: persist
            await _candidateRepository.UpdateAsync(candidate, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}