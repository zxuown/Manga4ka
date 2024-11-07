using FluentValidation;
using Manga4ka.Business.Models;

namespace Manga4ka.Business.Validation;

public class CommentValidator : AbstractValidator<CommentDto>
{
    public CommentValidator()
    {
        RuleFor(c => c.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Comment Id must be greater than or equal to 0");

        RuleFor(c => c.User)
            .NotNull().WithMessage("User is required");

        RuleFor(c => c.Manga)
            .NotNull().WithMessage("Manga is required");

        RuleFor(c => c.Text)
            .NotEmpty().WithMessage("Comment text is required")
            .MaximumLength(1000).WithMessage("Comment text cannot be longer than 1000 characters");

        RuleFor(m => m.DatePublished)
            .NotEmpty().WithMessage("Date Published is required")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Date Published cannot be in the future");
    }
}
