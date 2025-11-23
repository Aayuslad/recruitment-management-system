using FluentValidation;

using Server.Application.Aggregates.Designations.Queries;

namespace Server.Application.Aggregates.Designations.Validators
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