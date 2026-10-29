using FluentValidation;
using MovieFlix.Application.Contracts.ReleaseYears.Requests;

namespace MovieFlix.Application.Contracts.ReleaseYears.Validators;

public class CreateReleaseYearValidator : AbstractValidator<CreateReleaseYearRequest>
{
    public CreateReleaseYearValidator()
    {
        RuleFor(x => x.Year)
            .InclusiveBetween(1888, DateTime.UtcNow.Year + 5)
            .WithMessage($"Year must be between 1888 and {DateTime.UtcNow.Year + 5}");
    }
}