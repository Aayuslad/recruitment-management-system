using MediatR;

using Server.Application.Aggregates.Designations.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Designations.Commands
{
    public class CreateDesignationCommand : IRequest<Result>
    {
        public CreateDesignationCommand(string name, List<DesignationSkillDTO> designationSkills)
        {
            Name = name;
            DesignationSkills = designationSkills;
        }

        public string Name { get; set; }
        public List<DesignationSkillDTO> DesignationSkills { get; set; } =
            new List<DesignationSkillDTO>();
    }
}