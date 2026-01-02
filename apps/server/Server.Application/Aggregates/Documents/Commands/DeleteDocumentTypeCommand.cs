using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Documents.Commands
{
    public class DeleteDocumentTypeCommand : IRequest<Result>
    {
        public DeleteDocumentTypeCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}