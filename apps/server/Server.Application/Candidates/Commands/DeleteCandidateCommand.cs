using MediatR;

using Server.Core.Results;

namespace Server.Application.Candidates.Commands
{
    public class DeleteCandidateCommand : IRequest<Result>
    {
        public DeleteCandidateCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}