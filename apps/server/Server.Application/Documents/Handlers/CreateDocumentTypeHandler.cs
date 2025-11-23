using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Documents.Commands;
using Server.Application.Exeptions;
using Server.Core.Results;
using Server.Domain.Entities;

namespace Server.Application.Documents.Handlers
{
    internal class CreateDocumentTypeHandler : IRequestHandler<CreateDocumentTypeCommand, Result>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateDocumentTypeHandler(IHttpContextAccessor httpContextAccessor, IDocumentRepository documentRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _documentRepository = documentRepository;
        }

        public async Task<Result> Handle(CreateDocumentTypeCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedExeption();
            }

            // step 1: check if with this name a doc tyype exsist
            var result = await _documentRepository.ExistsByNameAsync(request.Name, cancellationToken);
            if (result)
            {
                throw new ConflictExeption($"Document type with name {request.Name} already exsist");
            }

            // step 2: create entity
            var docType = DocumentType.Create(request.Name, Guid.Parse(userIdString));

            // step 3: persist entity
            await _documentRepository.AddAsync(docType, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}