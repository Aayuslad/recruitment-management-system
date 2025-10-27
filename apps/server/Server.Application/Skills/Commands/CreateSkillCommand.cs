using MediatR;

using Server.Core.Results;

namespace Server.Application.Skills.Commands
{
    public class CreateSkillCommand: IRequest<Result>
    {
        public CreateSkillCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
