using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Roles.Commands
{
    public class DeleteRoleCommand : IRequest<Result>
    {
        public DeleteRoleCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}