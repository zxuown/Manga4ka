using FluentValidation;
using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Data.Interfaces;

namespace Manga4ka.Business.Validation;

public class AuthorValidator : AbstractValidator<AuthorDto>
{
    private IAuthorService  _service { get; }
    public AuthorValidator(IAuthorService authorService)
    {
        _service = authorService;

        When(m => m.Id > 0, () =>
        {
            RuleFor(m => m.Id)
                .MustAsync(AuthorExists).WithMessage("Author not found for update");
        });

        RuleFor(x => x.Id)
            .Equal(0).When(m => m.Id == 0).WithMessage("Author Id must be 0 when creating a new author");

        RuleFor(a => a.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Author Id must be greater than or equal to 0");


        RuleFor(a => a.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Author Id must be greater than or equal to 0");

        RuleFor(a => a.Name)
            .NotEmpty().WithMessage("Author name is required")
            .MaximumLength(50).WithMessage("Author name cannot be longer than 50 characters");

        RuleFor(a => a.Lastname)
            .NotEmpty().WithMessage("Author lastname is required")
            .MaximumLength(50).WithMessage("Author lastname cannot be longer than 50 characters");

        RuleFor(a => a.Description)
            .NotEmpty().WithMessage("Author description is required")
            .MaximumLength(500).WithMessage("Description cannot be longer than 500 characters");

        RuleFor(a => a.DateOfBirth)
            .NotEmpty().WithMessage("Date Of Birth is required")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Date of birth must be in the past");

        RuleFor(a => a.Image)
            .Must(BeAValidUrl).When(a => !string.IsNullOrEmpty(a.Image)).WithMessage("Invalid Image URL");
    }
    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    private async Task<bool> AuthorExists(int authorId, CancellationToken cancellationToken)
    {
        var author = await _service.GetAuthorByIdAsync(authorId);
        return author != null;
    }
}
