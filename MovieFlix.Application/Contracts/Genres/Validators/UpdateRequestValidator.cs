using FluentValidation;
using MovieFlix.Application.Contracts.Genres.Requests;

namespace MovieFlix.Application.Contracts.Genres.Validators;

public class UpdateRequestValidator : AbstractValidator<UpdateGenreRequest>
{
    public UpdateRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Janr nomi bo'sh bo'lmasligi kerak.")
            .Length(2, 256).WithMessage("Janr nomi 2 dan 256 tagacha belgidan iborat bo'lishi kerak.");

        When(x => !string.IsNullOrWhiteSpace(x.Slug), () =>
        {
            RuleFor(x => x.Slug)
                .Length(2, 256).WithMessage("Janr identifikatori 2 dan 256 tagacha belgidan iborat bo'lishi kerak.");
        });
    }
}