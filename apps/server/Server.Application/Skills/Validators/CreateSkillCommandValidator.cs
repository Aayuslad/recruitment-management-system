using FluentValidation;

using Server.Application.Skills.Commands;

namespace Server.Application.Skills.Validators
{
    internal class CreateSkillCommandValidator : AbstractValidator<CreateSkillCommand>
    {
        public CreateSkillCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Skill name is required")
                .MaximumLength(50).WithMessage("Skill name must be at most 50 characters long.");
        }
    }
}