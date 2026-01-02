using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Documents.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Documents.Handlers
{
    internal class EditDocumentTypeHandler : IRequestHandler<EditDocumentTypeCommand, Result>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserContext _userContext;

        public EditDocumentTypeHandler(IUserContext userContext, IDocumentRepository documentRepository)
        {
            _userContext = userContext;
            _documentRepository = documentRepository;
        }

        public async Task<Result> Handle(EditDocumentTypeCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch the doc type
            var documentType = await _documentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (documentType == null)
            {
                throw new NotFoundException("Document Type Not Found");
            }

            // step 2: update the entityO
            documentType.Update(
                name: request.Name,
                updatedBy: _userContext.UserId
            );

            // step 3: persist entity
            await _documentRepository.UpdateAsync(documentType, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}