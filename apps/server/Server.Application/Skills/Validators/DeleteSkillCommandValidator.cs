using FluentValidation;

using Server.Application.Skills.Commands;

namespace Server.Application.Skills.Validators
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