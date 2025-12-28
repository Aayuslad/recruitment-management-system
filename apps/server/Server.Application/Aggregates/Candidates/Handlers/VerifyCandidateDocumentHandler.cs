
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Candidates.Commands;
using Server.Application.Exeptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Handlers
{
    internal class VerifyCandidateDocumentHandler : IRequestHandler<VerifyCandidateDocumentCommand, Result>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VerifyCandidateDocumentHandler(ICandidateRepository candidateRepository, IHttpContextAccessor contextAccessor)
        {
            _candidateRepository = candidateRepository;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(VerifyCandidateDocumentCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedExeption();
            }

            // step 1: fetch the entity
            var candidate = await _candidateRepository.GetByIdAsync(request.CandidateId, cancellationToken);
            if (candidate == null)
            {
                return Result.Failure("Candidate not found");
            }

            // step 2: create doc entity
            var docToVerify = candidate.Documents.FirstOrDefault(x => x.Id == request.DocumentId);
            if (docToVerify == null)
            {
                return Result.Failure("Document not found");
            }

            docToVerify.MarkVerified(Guid.Parse(userIdString));

            // step 3: persist entity
            await _candidateRepository.UpdateAsync(candidate, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}