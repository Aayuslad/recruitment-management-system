using MediatR;

using Server.Core.Results;

namespace Server.Application.Skills.Commands
{
    public class EditSkillCommand: IRequest<Result>
    {
        public EditSkillCommand(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
