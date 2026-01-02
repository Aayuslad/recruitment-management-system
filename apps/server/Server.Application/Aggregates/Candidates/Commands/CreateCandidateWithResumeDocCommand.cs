using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Commands
{
    public class CreateCandidateWithResumeDocCommand : IRequest<Result>
    {
    }
}