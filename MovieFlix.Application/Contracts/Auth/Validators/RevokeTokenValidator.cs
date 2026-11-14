using FluentValidation;
using MovieFlix.Application.Contracts.Auth.Requests;

namespace MovieFlix.Application.Contracts.Auth.Validators;

public class RevokeTokenValidator : AbstractValidator<RevokeTokenRequest>
{
    public RevokeTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}
