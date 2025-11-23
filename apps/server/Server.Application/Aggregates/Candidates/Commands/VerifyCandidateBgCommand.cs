using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Commands
{
    public class VerifyCandidateBgCommand : IRequest<Result>
    {
        public VerifyCandidateBgCommand(Guid id)
        {
            CandidateId = id;
        }

        public Guid CandidateId { get; set; }
    }
}