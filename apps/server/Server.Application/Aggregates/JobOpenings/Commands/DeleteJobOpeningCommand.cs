using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.JobOpenings.Commands
{
    public class DeleteJobOpeningCommand : IRequest<Result>
    {
        public DeleteJobOpeningCommand(Guid id)
        {
            JobOpeningId = id;
        }

        public Guid JobOpeningId { get; set; }
    }
}