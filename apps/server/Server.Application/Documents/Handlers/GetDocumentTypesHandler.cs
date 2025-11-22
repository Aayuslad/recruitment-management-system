using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Documents.Queries;
using Server.Application.Documents.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Documents.Handlers
{
    internal class GetDocumentTypesHandler : IRequestHandler<GetDocumentTypesQuery, Result<List<DocumentDetailDTO>>>
    {
        private readonly IDocumentRepository _documentRepository;

        public GetDocumentTypesHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<Result<List<DocumentDetailDTO>>> Handle(GetDocumentTypesQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch all
            var docTypes = await _documentRepository.GetAllAsync(cancellationToken);

            // step 2: list dtos
            var docTypeDtos = docTypes.Select(
                selector: x => new DocumentDetailDTO
                {
                    Id = x.Id,
                    Name = x.Name
                }
            ).ToList();

            // step 3: return result
            return Result<List<DocumentDetailDTO>>.Success(docTypeDtos);
        }
    }
}