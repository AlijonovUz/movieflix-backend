using FluentValidation;
using MovieFlix.Application.Contracts.Countries.Requests;

namespace MovieFlix.Application.Contracts.Countries.Validators;

public class CreateRequestValidator : AbstractValidator<CreateCountryRequest>
{
    public CreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 256).WithMessage("Name must be between 2 and 256 characters long.");
        
        When(x => !string.IsNullOrEmpty(x.Slug), () =>
            RuleFor(x => x.Slug)
                .Length(2, 256).WithMessage("Slug must be between 2 and 256 characters long.")
        );
    }
}