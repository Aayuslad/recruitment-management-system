
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Candidates.Commands;
using Server.Core.Results;
using Server.Domain.Entities.Candidates;

namespace Server.Application.Aggregates.Candidates.Handlers
{
    internal class AddCandidateDocumentHandler : IRequestHandler<AddCandidateDocumentCommand, Result>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUserContext _userContext;

        public AddCandidateDocumentHandler(ICandidateRepository candidateRepository, IUserContext userContext)
        {
            _candidateRepository = candidateRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(AddCandidateDocumentCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch the entity
            var candidate = await _candidateRepository.GetByIdAsync(request.Id, cancellationToken);
            if (candidate == null)
            {
                return Result.Failure("Candidate not found");
            }

            // step 2: create doc entity
            var newDoc = CandidateDocument.Create(
                id: null,
                candidateId: candidate.Id,
                documentTypeId: request.DocumentTypeId,
                url: request.Url
            );

            candidate.AddDocument(newDoc);

            // step 3: persist entity
            await _candidateRepository.UpdateAsync(candidate, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}