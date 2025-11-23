using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Skills.Commands
{
    public class DeleteSkillCommand : IRequest<Result>
    {
        public DeleteSkillCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}