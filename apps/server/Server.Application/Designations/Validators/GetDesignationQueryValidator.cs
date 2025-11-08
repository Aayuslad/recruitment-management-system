using FluentValidation;

using Server.Application.Designations.Queries;

namespace Server.Application.Designations.Validators
{
    internal class GetDesignationQueryValidator : AbstractValidator<GetDesignationQuery>
    {
        public GetDesignationQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Designation id is required.");
        }
    }
}