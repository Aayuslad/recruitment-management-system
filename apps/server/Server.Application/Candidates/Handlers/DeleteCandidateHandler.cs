
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Candidates.Commands;
using Server.Core.Results;

namespace Server.Application.Candidates.Handlers
{
    internal class DeleteCandidateHandler : IRequestHandler<DeleteCandidateCommand, Result>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteCandidateHandler(ICandidateRepository candidateRepository, IHttpContextAccessor contextAccessor)
        {
            _candidateRepository = candidateRepository;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(DeleteCandidateCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: fetch the entity
            var candidate = await _candidateRepository.GetByIdAsync(request.Id, cancellationToken);
            if (candidate == null)
            {
                return Result.Failure("Candidate not found", 404);
            }

            // step 2: soft delete
            candidate.Delete(Guid.Parse(userIdString));

            // step 3: persist
            await _candidateRepository.UpdateAsync(candidate, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}