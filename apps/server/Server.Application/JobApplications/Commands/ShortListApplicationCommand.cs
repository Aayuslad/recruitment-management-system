using MediatR;

using Server.Core.Results;

namespace Server.Application.JobApplications.Commands
{
    public class ShortListApplicationCommand : IRequest<Result>
    {
        public ShortListApplicationCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}