using MediatR;

using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobApplications.Commands
{
    public class MoveJobApplicationStatusCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public JobApplicationStatus MoveTo { get; set; }
    }
}