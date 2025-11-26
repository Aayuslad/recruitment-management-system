using FluentValidation;

using Server.Application.Aggregates.Designations.Commands;

namespace Server.Application.Aggregates.Designations.Validators
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