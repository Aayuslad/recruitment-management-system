
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Candidates.Commands;
using Server.Application.Exeptions;
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
                throw new UnAuthorisedExeption();
            }

            // step 1: fetch the entity
            var candidate = await _candidateRepository.GetByIdAsync(request.Id, cancellationToken);
            if (candidate == null)
            {
                throw new NotFoundExeption("Candidate not found.");
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