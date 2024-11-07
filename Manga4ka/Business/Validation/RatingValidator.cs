using FluentValidation;
using Manga4ka.Business.Models;

namespace Manga4ka.Business.Validation;

public class RatingValidator : AbstractValidator<RatingDto>
{
    public RatingValidator()
    {
        RuleFor(r => r.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Rating Id must be greater than or equal to 0");

        RuleFor(r => r.User)
            .NotNull().WithMessage("User is required");

        RuleFor(r => r.Manga)
            .NotNull().WithMessage("Manga is required");

        RuleFor(r => r.Value)
            .InclusiveBetween(1, 5).WithMessage("Rating value must be between 1 and 5");
    }
}
