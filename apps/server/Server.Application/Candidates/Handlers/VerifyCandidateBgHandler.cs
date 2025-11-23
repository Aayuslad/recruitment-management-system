
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Candidates.Commands;
using Server.Application.Exeptions;
using Server.Core.Results;

namespace Server.Application.Candidates.Handlers
{
    internal class VerifyCandidateBgHandler : IRequestHandler<VerifyCandidateBgCommand, Result>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VerifyCandidateBgHandler(ICandidateRepository candidateRepository, IHttpContextAccessor contextAccessor)
        {
            _candidateRepository = candidateRepository;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(VerifyCandidateBgCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedExeption();
            }

            // step 1: fetch the candidate
            var candidate = await _candidateRepository.GetByIdAsync(request.CandidateId, cancellationToken);
            if (candidate == null)
            {
                throw new NotFoundExeption("Candidate Not Found");
            }

            // step 2: verify
            candidate.MarkBackgroundVerified(Guid.Parse(userIdString));

            // step 3: persist
            await _candidateRepository.UpdateAsync(candidate, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}