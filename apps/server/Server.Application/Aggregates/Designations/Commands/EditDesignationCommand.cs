using MediatR;

using Server.Application.Aggregates.Designations.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Designations.Commands
{
    public class EditDesignationCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public List<DesignationSkillDTO>? DesignationSkills { get; set; } = new();
    }
}