using FluentValidation;
using MovieFlix.Application.Contracts.Movies.Requests;

namespace MovieFlix.Application.Contracts.Movies.Validators;

public class UpdateMovieValidator : AbstractValidator<UpdateMovieRequest>
{
    public UpdateMovieValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(256).WithMessage("Title must not exceed 256 characters.");

        When(x => !string.IsNullOrWhiteSpace(x.Slug), () =>
        {
            RuleFor(x => x.Slug)
                .MaximumLength(256).WithMessage("Slug must not exceed 256 characters.");
        });

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Duration must be greater than 0 minutes.");

        RuleFor(x => x.Language)
            .MaximumLength(50).WithMessage("Language must not exceed 50 characters.");

        RuleFor(x => x.MovieType)
            .IsInEnum().WithMessage("Invalid movie type.");

        When(x => x.PosterFile is not null, () =>
        {
            RuleFor(x => x.PosterFile!.Length)
                .LessThanOrEqualTo(5 * 1024 * 1024)
                .WithMessage("Poster file size must not exceed 5MB.");

            RuleFor(x => x.PosterFile!.FileName)
                .Must(fileName =>
                {
                    var ext = Path.GetExtension(fileName).ToLowerInvariant();
                    return ext is ".jpg" or ".jpeg" or ".png" or ".webp";
                })
                .WithMessage("Only .jpg, .jpeg, .png, and .webp formats are allowed.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.PosterUrl), () =>
        {
            RuleFor(x => x.PosterUrl)
                .MaximumLength(5000).WithMessage("Poster URL must not exceed 5000 characters.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.TrailerUrl), () =>
        {
            RuleFor(x => x.TrailerUrl)
                .MaximumLength(5000).WithMessage("Trailer URL must not exceed 5000 characters.");
        });

        RuleFor(x => x.AgeLimitId)
            .GreaterThan(0).WithMessage("Age limit is required.");

        RuleFor(x => x.ReleaseYearId)
            .GreaterThan(0).WithMessage("Release year is required.");

        RuleFor(x => x.GenreIds)
            .NotEmpty().WithMessage("At least one genre must be selected.");

        RuleFor(x => x.CountryIds)
            .NotEmpty().WithMessage("At least one country must be selected.");
    }
}
