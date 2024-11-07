using FluentValidation;
using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Data.Interfaces;

namespace Manga4ka.Business.Validation;

public class GenreValidator : AbstractValidator<GenreDto>
{
    private IGenreService _service { get; }

    public GenreValidator(IGenreService genreService)
    {
        _service = genreService;

        When(m => m.Id > 0, () =>
        {
            RuleFor(m => m.Id)
                .MustAsync(GenreExists).WithMessage("Genre not found for update");
        });

        When(x => x.Id == 0, () =>
        {
            RuleFor(x => x.Id).Equal(0).WithMessage("Genre Id must be 0 when creating a new genre");
        });

        RuleFor(g => g.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Genre Id must be greater than or equal to 0");

        RuleFor(g => g.Title)
            .NotEmpty().WithMessage("Genre title is required")
            .MaximumLength(50).WithMessage("Genre title cannot be longer than 50 characters");

        RuleFor(g => g.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot be longer than 500 characters");

        RuleFor(g => g.Image)
            .Must(BeAValidUrl).When(g => !string.IsNullOrEmpty(g.Image)).WithMessage("Invalid Image URL");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    private async Task<bool> GenreExists(int genreId, CancellationToken cancellationToken)
    {
        var genre = await _service.GetGenreByIdAsync(genreId);
        return genre != null;
    }
}
