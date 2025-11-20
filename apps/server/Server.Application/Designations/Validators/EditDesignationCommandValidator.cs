using FluentValidation;

using Server.Application.Designations.Commands;

namespace Server.Application.Designations.Validators
{
    internal class EditDesignationCommandValidator : AbstractValidator<EditDesignationCommand>
    {
        public EditDesignationCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Designation id is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Designation name is required.")
                .MaximumLength(50).WithMessage("Designation name must not exceed 50 characters.");

            RuleForEach(x => x.DesignationSkills)
                .SetValidator(new DesignationSkillDTOValidator()!);
        }
    }
}