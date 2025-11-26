using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Skills.Commands
{
    public class CreateSkillCommand : IRequest<Result>
    {
        public CreateSkillCommand(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}