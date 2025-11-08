using FluentValidation;

using Server.Application.Designations.Commands;

namespace Server.Application.Designations.Validators
{
    internal class CreateDesignationCommandValidator : AbstractValidator<CreateDesignationCommand>
    {
        public CreateDesignationCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Designation name is required.")
                .MaximumLength(50).WithMessage("Designation name must not exceed 50 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Designation description is required.")
                .MaximumLength(500).WithMessage("Designation description must not exceed 500 characters.");

            RuleForEach(x => x.DesignationSkills)
                .SetValidator(new DesignationSkillDTOValidator()!);
        }
    }
}