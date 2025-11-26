using FluentValidation;

using Server.Application.Aggregates.Skills.Commands;

namespace Server.Application.Aggregates.Skills.Validators
{
    internal class DeleteSkillCommandValidator : AbstractValidator<DeleteSkillCommand>
    {
        public DeleteSkillCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Skill ID is required.");
        }
    }
}