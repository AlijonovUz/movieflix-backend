using FluentValidation;
using MovieFlix.Application.Contracts.Auth.Requests;

namespace MovieFlix.Application.Contracts.Auth.Validators;

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 32).WithMessage("Username must be between 3 and 32 characters long.")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores.");

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email format is invalid.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");
        });

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        When(x => !string.IsNullOrWhiteSpace(x.FirstName), () =>
        {
            RuleFor(x => x.FirstName)
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.LastName), () =>
        {
            RuleFor(x => x.LastName)
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");
        });
    }
}
