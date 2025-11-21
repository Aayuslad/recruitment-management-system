using FluentValidation;

using Server.Application.Users.Commands;

namespace Server.Application.Users.Validators
{
    internal class CreateUserProfileCommandValidator : AbstractValidator<CreateUserProfileCommand>
    {
        public CreateUserProfileCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must be at most 50 characters long.");

            RuleFor(x => x.MiddleName)
                .MaximumLength(50).WithMessage("Middle name must be at most 50 characters long.")
                .When(x => !string.IsNullOrEmpty(x.MiddleName));

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must be at most 50 characters long.");

            RuleFor(x => x.ContactNumber)
                .NotEmpty()
                .WithMessage("Contact number is required.");

            RuleFor(x => x.Gender);

            RuleFor(x => x.Dob)
                .NotEmpty()
                .WithMessage("Date of birth is required.")
                .Must(d => d.Kind == DateTimeKind.Utc)
                .WithMessage("Date of birth must be a UTC date.")
                .LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past.");
        }
    }
}