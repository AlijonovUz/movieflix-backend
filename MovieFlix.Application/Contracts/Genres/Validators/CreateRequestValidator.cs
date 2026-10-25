using FluentValidation;
using MovieFlix.Application.Contracts.Genres.Requests;

namespace MovieFlix.Application.Contracts.Genres.Validators;

public class CreateRequestValidator : AbstractValidator<CreateGenreRequest>
{
    public CreateRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 256).WithMessage("Name must be between 2 and 256 characters long.");

        When(x => !string.IsNullOrWhiteSpace(x.Slug), () =>
        {
            RuleFor(x => x.Slug)
                .Length(2, 256).WithMessage("Slug must be between 2 and 256 characters long.");
        });
    }
}