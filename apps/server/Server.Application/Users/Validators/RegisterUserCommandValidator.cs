using FluentValidation;

using Server.Application.Users.Commands;

namespace Server.Application.Users.Validators
{
    internal class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(20).WithMessage("Username must be at most 20 characters long.")
                .Matches(@"^[a-zA-Z0-9](?!.*[._]{2})[a-zA-Z0-9._]*[a-zA-Z0-9]$")
                .WithMessage("Username can contain letters, numbers, _ or ., no spaces, cannot start/end with _ or .");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(100).WithMessage("Password must be at most 100 characters long.")
                .Matches(@"^[A-Za-z\d@#$%&*!]{6,10}$")
                .WithMessage("Password can only include letters, digits, and @ # $ % & * !");
        }
    }
}