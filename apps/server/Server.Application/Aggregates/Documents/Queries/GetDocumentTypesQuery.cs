using MediatR;

using Server.Application.Aggregates.Documents.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Documents.Queries
{
    public class GetDocumentTypesQuery : IRequest<Result<List<DocumentDetailDTO>>>
    {
    }
}