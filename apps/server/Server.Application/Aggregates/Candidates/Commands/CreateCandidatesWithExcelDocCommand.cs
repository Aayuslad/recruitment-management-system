using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Commands
{
    public class CreateCandidatesWithExcelDocCommand : IRequest<Result>
    {
    }
}