using FluentValidation;

using Server.Application.Aggregates.Users.Commands;
using Server.Application.Aggregates.Users.Commands.DTOs;

namespace Server.Application.Aggregates.Users.Validators
{
    internal class EditUserRolesCommandValidator : AbstractValidator<EditUserRolesCommand>
    {
        public EditUserRolesCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.Roles)
                .SetValidator((IValidator<List<UserRolesDTO>>)new RolesDTOValidator()!);
        }
    }
}