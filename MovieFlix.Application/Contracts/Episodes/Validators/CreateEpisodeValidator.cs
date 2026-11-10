using FluentValidation;
using MovieFlix.Application.Contracts.Episodes.Requests;

namespace MovieFlix.Application.Contracts.Episodes.Validators;

public class CreateEpisodeValidator : AbstractValidator<CreateEpisodeRequest>
{
    public CreateEpisodeValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.MovieId)
            .GreaterThan(0).WithMessage("Movie is required.");

        RuleFor(x => x.VideoUrl)
            .NotEmpty().WithMessage("Video URL is required.")
            .MaximumLength(5000).WithMessage("Video URL must not exceed 5000 characters.");

        When(x => x.Number.HasValue, () =>
        {
            RuleFor(x => x.Number!.Value)
                .GreaterThan(0).WithMessage("Episode number must be greater than 0.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Title), () =>
        {
            RuleFor(x => x.Title)
                .MaximumLength(256).WithMessage("Episode title must not exceed 256 characters.");
        });
    }
}
