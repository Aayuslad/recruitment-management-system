using MediatR;

using Server.Application.Designations.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.Designations.Commands
{
    public class EditDesignationCommand : IRequest<Result>
    {
        public EditDesignationCommand(Guid id, string name, string description, List<DesignationSkillDTO>? designationSkills)
        {
            Id = id;
            Name = name;
            Description = description;
            DesignationSkills = designationSkills;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DesignationSkillDTO>? DesignationSkills { get; set; } = new();
    }
}