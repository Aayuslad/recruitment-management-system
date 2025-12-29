using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Commands
{
    public class AddCandidateDocumentCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public Guid DocumentTypeId { get; set; }
        public string Url { get; set; } = null!;
    }
}