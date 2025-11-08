using FluentValidation;

using Server.Application.Designations.Commands;

namespace Server.Application.Designations.Validators
{
    internal class DeleteDesignationCommandValidator : AbstractValidator<DeleteDesignationCommand>
    {
        public DeleteDesignationCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Designation ID is required.");
        }
    }
}