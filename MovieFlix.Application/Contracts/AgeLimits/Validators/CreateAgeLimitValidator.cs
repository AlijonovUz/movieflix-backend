using FluentValidation;
using MovieFlix.Application.Contracts.AgeLimits.Requests;

namespace MovieFlix.Application.Contracts.AgeLimits.Validators;

public class CreateAgeLimitValidator : AbstractValidator<CreateAgeLimitRequest>
{
    public CreateAgeLimitValidator()
    {
        RuleFor(x => x.Age)
            .InclusiveBetween(0, 100).WithMessage("Age must be between 0 and 100");
    }
}