using FluentValidation;
using MovieFlix.Application.Contracts.Reviews.Requests;

namespace MovieFlix.Application.Contracts.Reviews.Validators;

public class CreateReviewValidator : AbstractValidator<CreateReviewRequest>
{
    public CreateReviewValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.MovieId)
            .GreaterThan(0).WithMessage("Movie is required.");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required.")
            .MaximumLength(1000).WithMessage("Comment must not exceed 1000 characters.");

        When(x => x.Rating.HasValue, () =>
        {
            RuleFor(x => x.Rating!.Value)
                .InclusiveBetween(1, 10).WithMessage("Rating must be between 1 and 10.");
        });

        When(x => x.ParentId.HasValue, () =>
        {
            RuleFor(x => x.ParentId!.Value)
                .GreaterThan(0).WithMessage("Parent review ID must be greater than 0.");
        });
    }
}
