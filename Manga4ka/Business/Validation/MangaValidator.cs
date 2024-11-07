using FluentValidation;
using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Data.Interfaces;
using Manga4ka.Data.Repositories;
using System.Text.RegularExpressions;

namespace Manga4ka.Business.Validation;

public class MangaValidator : AbstractValidator<MangaDto>
{
    private IMangaService _service { get; }
    public MangaValidator(IMangaService mangaService)
    {
        _service = mangaService;

        When(m => m.Id > 0, () =>
        {
            RuleFor(m => m.Id)
                .MustAsync(MangaExists).WithMessage("Manga not found for update");
        });

        RuleFor(x => x.Id)
           .Equal(0).When(m => m.Id == 0).WithMessage("Manga Id must be 0 when creating a new manga");

        RuleFor(a => a.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Manga Id must be greater than or equal to 0");

        RuleFor(m => m.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title cannot be longer than 100 characters");

        RuleFor(m => m.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000).WithMessage("Description cannot be longer than 1000 characters");

        RuleFor(m => m.DatePublished)
            .NotEmpty().WithMessage("Date Published is required")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Date Published cannot be in the future");

        RuleFor(m => m.Author)
            .NotNull().WithMessage("Author is required");

        RuleFor(m => m.Image)
            .Must(BeAValidUrl).When(m => !string.IsNullOrEmpty(m.Image)).WithMessage("Invalid Image URL");

        RuleFor(m => m.Pdfile)
            .Must(BeAValidPdfFilePath)
            .When(m => !string.IsNullOrEmpty(m.Pdfile))
            .WithMessage("Invalid PDF file path");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    private bool BeAValidPdfFilePath(string path)
    {
        var regex = new Regex(@"^PdfFiles\\[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}\.pdf$");
        return regex.IsMatch(path);
    }

    private async Task<bool> MangaExists(int mangaId, CancellationToken cancellationToken)
    {
        var manga = await _service.GetMangaByIdAsync(mangaId);
        return manga != null;
    }
}
