using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Commands
{
    public class VerifyCandidateDocumentCommand : IRequest<Result>
    {
        public VerifyCandidateDocumentCommand(Guid candidateId, Guid documentId)
        {
            CandidateId = candidateId;
            DocumentId = documentId;
        }

        public Guid CandidateId { get; set; }
        public Guid DocumentId { get; set; }
    }
}