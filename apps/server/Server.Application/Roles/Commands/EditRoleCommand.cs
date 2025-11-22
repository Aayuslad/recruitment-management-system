using MediatR;

using Server.Core.Results;

namespace Server.Application.Roles.Commands
{
    public class EditRoleCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
    }
}