using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Roles.Commands
{
    public class CreateRoleCommand : IRequest<Result>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
    }
}