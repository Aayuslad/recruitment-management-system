using MediatR;

using Server.Application.Designations.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.Designations.Commands
{
    public class CreateDesignationCommand : IRequest<Result>
    {
        public CreateDesignationCommand(string name, string description, List<DesignationSkillDTO>? designationSkills)
        {
            Name = name;
            Description = description;
            DesignationSkills = designationSkills;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<DesignationSkillDTO>? DesignationSkills { get; set; } = new();
    }
}