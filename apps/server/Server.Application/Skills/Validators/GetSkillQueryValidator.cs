using FluentValidation;

using Server.Application.Skills.Queries;

namespace Server.Application.Skills.Validators
{
    public class GetSkillQueryValidator : AbstractValidator<GetSkillQuery>
    {
        public GetSkillQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}