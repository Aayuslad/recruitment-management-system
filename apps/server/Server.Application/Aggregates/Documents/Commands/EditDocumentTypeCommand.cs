using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Documents.Commands
{
    public class EditDocumentTypeCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}