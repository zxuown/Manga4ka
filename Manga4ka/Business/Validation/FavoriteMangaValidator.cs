using FluentValidation;
using Manga4ka.Business.Models;

namespace Manga4ka.Business.Validation;

public class FavoriteMangaValidator : AbstractValidator<FavoriteMangaDto>
{
    public FavoriteMangaValidator()
    {
        RuleFor(fm => fm.Id)
            .GreaterThanOrEqualTo(0).WithMessage("FavoriteManga Id must be greater than or equal to 0");

        RuleFor(fm => fm.Manga)
            .NotNull().WithMessage("Manga is required");

        RuleFor(fm => fm.User)
            .NotNull().WithMessage("User is required");

        RuleFor(fm => fm.IsFavorite)
            .NotNull().WithMessage("IsFavorite status is required");
    }
}
