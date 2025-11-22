using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Documents.Commands;
using Server.Core.Results;

namespace Server.Application.Documents.Handlers
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
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: fetch the entity
            var docType = await _documentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (docType is null)
            {
                return Result.Failure("Document type does not exist", 404);
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
