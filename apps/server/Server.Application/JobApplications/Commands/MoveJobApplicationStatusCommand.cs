using MediatR;

using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.JobApplications.Commands
{
    public class MoveJobApplicationStatusCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public JobApplicationStatus MoveTo { get; set; }
    }
}