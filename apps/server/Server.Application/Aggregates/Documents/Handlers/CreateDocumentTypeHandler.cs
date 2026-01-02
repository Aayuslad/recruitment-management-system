using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Documents.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities.Documents;

namespace Server.Application.Aggregates.Documents.Handlers
{
    internal class CreateDocumentTypeHandler : IRequestHandler<CreateDocumentTypeCommand, Result>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserContext _userContext;

        public CreateDocumentTypeHandler(IUserContext userContext, IDocumentRepository documentRepository)
        {
            _userContext = userContext;
            _documentRepository = documentRepository;
        }

        public async Task<Result> Handle(CreateDocumentTypeCommand request, CancellationToken cancellationToken)
        {
            // step 1: check if with this name a doc tyype exsist
            var result = await _documentRepository.ExistsByNameAsync(request.Name, cancellationToken);
            if (result)
            {
                throw new ConflictException($"Document type with name {request.Name} already exsist");
            }

            // step 2: create entity
            var docType = DocumentType.Create(request.Name, _userContext.UserId);

            // step 3: persist entity
            await _documentRepository.AddAsync(docType, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}