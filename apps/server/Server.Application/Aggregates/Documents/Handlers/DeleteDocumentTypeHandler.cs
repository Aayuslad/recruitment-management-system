using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Documents.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Documents.Handlers
{
    internal class DeleteDocumentTypeHandler : IRequestHandler<DeleteDocumentTypeCommand, Result>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteDocumentTypeHandler(IHttpContextAccessor httpContextAccessor, IDocumentRepository documentRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _documentRepository = documentRepository;
        }

        public async Task<Result> Handle(DeleteDocumentTypeCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedException();
            }

            // step 1: fetch the entity
            var docType = await _documentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (docType is null)
            {
                throw new NotFoundException($"Document Type not found.");
            }

            // step 2: soft delete
            docType.Delete(Guid.Parse(userIdString));

            // step 3: persist
            await _documentRepository.UpdateAsync(docType, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}