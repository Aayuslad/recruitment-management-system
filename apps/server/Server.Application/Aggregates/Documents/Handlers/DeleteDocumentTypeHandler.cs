using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Documents.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Documents.Handlers
{
    internal class DeleteDocumentTypeHandler : IRequestHandler<DeleteDocumentTypeCommand, Result>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserContext _userContext;

        public DeleteDocumentTypeHandler(IUserContext userContext, IDocumentRepository documentRepository)
        {
            _userContext = userContext;
            _documentRepository = documentRepository;
        }

        public async Task<Result> Handle(DeleteDocumentTypeCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch the entity
            var docType = await _documentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (docType is null)
            {
                throw new NotFoundException($"Document Type not found.");
            }

            // step 2: soft delete
            docType.Delete(_userContext.UserId);

            // step 3: persist
            await _documentRepository.UpdateAsync(docType, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}