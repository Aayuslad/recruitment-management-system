using FluentValidation;

using Server.Application.Aggregates.Designations.Commands.DTOs;

namespace Server.Application.Aggregates.Designations.Validators
{
    internal class DesignationSkillDTOValidator : AbstractValidator<DesignationSkillDTO>
    {
        public DesignationSkillDTOValidator()
        {
            RuleFor(x => x.SkillId)
                .NotEmpty().WithMessage("Skill ID is required.");

            RuleFor(x => x.SkillType)
                .NotEmpty().WithMessage("Skill type is required.")
                .IsInEnum().WithMessage("Invalid skill type.");

            RuleFor(x => x.MinExperienceYears)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum experience years must be non-negative.");
        }
    }
}