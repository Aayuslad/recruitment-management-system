
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Candidates.Commands;
using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Handlers
{
    internal class VerifyCandidateDocumentHandler : IRequestHandler<VerifyCandidateDocumentCommand, Result>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUserContext _userContext;

        public VerifyCandidateDocumentHandler(ICandidateRepository candidateRepository, IUserContext userContext)
        {
            _candidateRepository = candidateRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(VerifyCandidateDocumentCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch the entity
            var candidate = await _candidateRepository.GetByIdAsync(request.CandidateId, cancellationToken);
            if (candidate == null)
            {
                return Result.Failure("Candidate not found");
            }

            // step 2: fetch doc entity
            var docToVerify = candidate.Documents.FirstOrDefault(x => x.Id == request.DocumentId);
            if (docToVerify == null)
            {
                return Result.Failure("Document not found");
            }

            // step 3: verify
            docToVerify.MarkVerified(_userContext.UserId);

            // step 4: persist entity
            await _candidateRepository.UpdateAsync(candidate, cancellationToken);

            // step 5: return result
            return Result.Success();
        }
    }
}