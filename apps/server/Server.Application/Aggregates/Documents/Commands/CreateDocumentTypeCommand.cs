using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Documents.Commands
{
    public class CreateDocumentTypeCommand : IRequest<Result>
    {
        public string Name { get; set; } = null!;
    }
}