using MediatR;

using Server.Core.Results;

namespace Server.Application.Documents.Commands
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