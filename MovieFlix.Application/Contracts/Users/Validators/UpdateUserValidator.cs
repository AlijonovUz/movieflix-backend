using FluentValidation;
using MovieFlix.Application.Contracts.Users.Requests;

namespace MovieFlix.Application.Contracts.Users.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator()
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

        When(x => x.Role.HasValue, () =>
        {
            RuleFor(x => x.Role!.Value)
                .IsInEnum().WithMessage("Invalid user role.");
        });
    }
}
