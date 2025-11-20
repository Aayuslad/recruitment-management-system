using MediatR;

using Server.Application.Designations.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.Designations.Commands
{
    public class EditDesignationCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public List<DesignationSkillDTO>? DesignationSkills { get; set; } = new();
    }
}