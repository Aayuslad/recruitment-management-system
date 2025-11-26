using MediatR;

using Server.Application.Aggregates.Users.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Users.Commands
{
    public class EditUserRolesCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
        public List<UserRolesDTO> Roles { get; set; } = new List<UserRolesDTO>();
    }
}