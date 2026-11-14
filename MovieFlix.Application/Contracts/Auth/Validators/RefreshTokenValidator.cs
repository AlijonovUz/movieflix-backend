using FluentValidation;
using MovieFlix.Application.Contracts.Auth.Requests;

namespace MovieFlix.Application.Contracts.Auth.Validators;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}
