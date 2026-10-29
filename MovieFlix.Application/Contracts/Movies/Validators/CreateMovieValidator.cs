using FluentValidation;
using MovieFlix.Application.Contracts.Movies.Requests;

namespace MovieFlix.Application.Contracts.Movies.Validators;

public class CreateMovieValidator : AbstractValidator<CreateMovieRequest>
{
    public CreateMovieValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Film sarlavhasi kiritilishi shart.")
            .MaximumLength(256).WithMessage("Film sarlavhasi 256 ta belgidan oshmasligi kerak.");

        When(x => !string.IsNullOrWhiteSpace(x.Slug), () =>
        {
            RuleFor(x => x.Slug)
                .MaximumLength(256).WithMessage("Slug 256 ta belgidan oshmasligi kerak.");
        });

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Film tavsifi 2000 ta belgidan oshmasligi kerak.");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Film davomiyligi 0 dan katta bo'lishi kerak.");

        RuleFor(x => x.Language)
            .MaximumLength(50).WithMessage("Film tili 50 ta belgidan oshmasligi kerak.");

        RuleFor(x => x.MovieType)
            .IsInEnum().WithMessage("Noto'g'ri film turi tanlandi.");

        RuleFor(x => x)
            .Must(x => x.PosterFile is not null || !string.IsNullOrWhiteSpace(x.PosterUrl))
            .WithMessage("Poster fayli yoki poster havolasi kiritilishi shart.");

        When(x => x.PosterFile is not null, () =>
        {
            RuleFor(x => x.PosterFile!.Length)
                .LessThanOrEqualTo(5 * 1024 * 1024)
                .WithMessage("Poster fayl hajmi 5MB dan oshmasligi kerak.");

            RuleFor(x => x.PosterFile!.FileName)
                .Must(fileName =>
                {
                    var ext = Path.GetExtension(fileName).ToLowerInvariant();
                    return ext is ".jpg" or ".jpeg" or ".png" or ".webp";
                })
                .WithMessage("Faqat .jpg, .jpeg, .png, .webp formatidagi rasmlar qabul qilinadi.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.PosterUrl), () =>
        {
            RuleFor(x => x.PosterUrl)
                .MaximumLength(5000).WithMessage("Poster havolasi 5000 ta belgidan oshmasligi kerak.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.TrailerUrl), () =>
        {
            RuleFor(x => x.TrailerUrl)
                .MaximumLength(5000).WithMessage("Treyler havolasi 5000 ta belgidan oshmasligi kerak.");
        });

        RuleFor(x => x.AgeLimitId)
            .GreaterThan(0).WithMessage("Yosh chegarasi tanlanishi shart.");

        RuleFor(x => x.ReleaseYearId)
            .GreaterThan(0).WithMessage("Chiqarilgan yili tanlanishi shart.");

        RuleFor(x => x.GenreIds)
            .NotEmpty().WithMessage("Kamida bitta janr tanlanishi shart.");

        RuleFor(x => x.CountryIds)
            .NotEmpty().WithMessage("Kamida bitta davlat tanlanishi shart.");
    }
}
