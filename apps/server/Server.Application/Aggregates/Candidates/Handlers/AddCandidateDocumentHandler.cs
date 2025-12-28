
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Candidates.Commands;
using Server.Application.Exeptions;
using Server.Core.Results;
using Server.Domain.Entities.Candidates;

namespace Server.Application.Aggregates.Candidates.Handlers
{
    internal class AddCandidateDocumentHandler : IRequestHandler<AddCandidateDocumentCommand, Result>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddCandidateDocumentHandler(ICandidateRepository candidateRepository, IHttpContextAccessor contextAccessor)
        {
            _candidateRepository = candidateRepository;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(AddCandidateDocumentCommand request, CancellationToken cancellationToken)
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
                return Result.Failure("Candidate not found");
            }

            // step 2: create doc entity
            var newDoc = CandidateDocument.Create(
                id: null,
                candidateId: candidate.Id,
                documentTypeId: request.DocumentTypeId,
                url: request.Url
            );

            candidate.AddDocumet(newDoc);

            // step 3: persist entity
            await _candidateRepository.UpdateAsync(candidate, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}