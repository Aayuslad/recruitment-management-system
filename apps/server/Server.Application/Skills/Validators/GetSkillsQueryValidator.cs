using FluentValidation;

using Server.Application.Skills.Queries;

namespace Server.Application.Skills.Validators
{
    public class GetSkillsQueryValidator : AbstractValidator<GetSkillsQuery>
    {
        public GetSkillsQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1)
                .When(x => x.Page.HasValue)
                .WithMessage("Page number must be greater than or equal to 1.");

            RuleFor(x => x.PageCount)
                .InclusiveBetween(1, 100)
                .When(x => x.PageCount.HasValue)
                .WithMessage("Page count must be between 1 and 100.");

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Search))
                .WithMessage("Search query must not exceed 100 characters.");
        }
    }
}