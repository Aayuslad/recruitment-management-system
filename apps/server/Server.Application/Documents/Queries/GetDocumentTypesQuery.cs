using MediatR;

using Server.Application.Documents.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Documents.Queries
{
    public class GetDocumentTypesQuery : IRequest<Result<List<DocumentDetailDTO>>>
    {
    }
}
