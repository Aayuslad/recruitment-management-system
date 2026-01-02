using FluentValidation;

using Server.Application.Aggregates.Skills.Commands;

namespace Server.Application.Aggregates.Skills.Validators
{
    internal class EditSkillCommandValidator : AbstractValidator<EditSkillCommand>
    {
        public EditSkillCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Skill ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Skill name is required")
                .MaximumLength(50).WithMessage("Skill name must be at most 50 characters long.");
        }
    }
}