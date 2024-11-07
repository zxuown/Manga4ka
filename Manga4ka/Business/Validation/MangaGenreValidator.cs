using FluentValidation;
using Manga4ka.Business.Models;

namespace Manga4ka.Business.Validation;

public class MangaGenreValidator : AbstractValidator<MangaGenreDto>
{
    public MangaGenreValidator()
    {
        RuleFor(mg => mg.Id)
            .GreaterThanOrEqualTo(0).WithMessage("MangaGenre Id must be greater than or equal to 0");

        RuleFor(mg => mg.Manga)
            .NotNull().WithMessage("Manga is required");

        RuleFor(mg => mg.Genre)
            .NotNull().WithMessage("Genre is required");
    }
}
