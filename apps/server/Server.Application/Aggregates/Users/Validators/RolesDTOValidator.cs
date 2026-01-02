using FluentValidation;

using Server.Application.Aggregates.Users.Commands.DTOs;

namespace Server.Application.Aggregates.Users.Validators
{
    internal class RolesDTOValidator : AbstractValidator<UserRolesDTO>
    {
        public RolesDTOValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role ID is required.");
        }
    }
}