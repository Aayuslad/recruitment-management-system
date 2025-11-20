using MediatR;

using Server.Core.Results;

namespace Server.Application.Skills.Commands
{
    public class EditSkillCommand : IRequest<Result>
    {
        public EditSkillCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}