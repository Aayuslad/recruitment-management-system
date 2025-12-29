using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Documents.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Documents.Handlers
{
    internal class EditDocumentTypeHandler : IRequestHandler<EditDocumentTypeCommand, Result>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditDocumentTypeHandler(IHttpContextAccessor httpContextAccessor, IDocumentRepository documentRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _documentRepository = documentRepository;
        }

        public async Task<Result> Handle(EditDocumentTypeCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedException();
            }

            // step 1: fetch the doc type
            var documentType = await _documentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (documentType == null)
            {
                throw new NotFoundException("Document Type Not Found");
            }

            // step 2: update the entityO
            documentType.Update(
                name: request.Name,
                updatedBy: Guid.Parse(userIdString)
            );

            // step 3: persist entity
            await _documentRepository.UpdateAsync(documentType, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}