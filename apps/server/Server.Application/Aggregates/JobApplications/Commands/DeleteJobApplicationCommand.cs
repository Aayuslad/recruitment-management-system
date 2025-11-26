using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.JobApplications.Commands
{
    public class DeleteJobApplicationCommand : IRequest<Result>
    {
        public DeleteJobApplicationCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}