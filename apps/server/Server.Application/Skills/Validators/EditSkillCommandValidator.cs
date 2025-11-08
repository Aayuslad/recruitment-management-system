using FluentValidation;

using Server.Application.Skills.Commands;

namespace Server.Application.Skills.Validators
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

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(400).WithMessage("Description must be at most 400 characters long.");
        }
    }
}