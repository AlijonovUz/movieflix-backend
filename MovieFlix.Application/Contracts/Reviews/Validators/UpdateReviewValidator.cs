using FluentValidation;
using MovieFlix.Application.Contracts.Reviews.Requests;

namespace MovieFlix.Application.Contracts.Reviews.Validators;

public class UpdateReviewValidator : AbstractValidator<UpdateReviewRequest>
{
    public UpdateReviewValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required.")
            .MaximumLength(1000).WithMessage("Comment must not exceed 1000 characters.");

        When(x => x.Rating.HasValue, () =>
        {
            RuleFor(x => x.Rating!.Value)
                .InclusiveBetween(1, 10).WithMessage("Rating must be between 1 and 10.");
        });
    }
}
