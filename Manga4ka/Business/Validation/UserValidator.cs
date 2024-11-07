using FluentValidation;
using Manga4ka.Business.Models;

namespace Manga4ka.Business.Validation;

public class UserValidator : AbstractValidator<UserDto>
{
    public UserValidator()
    {
        RuleFor(u => u.Id)
            .GreaterThanOrEqualTo(0).WithMessage("User Id must be greater than or equal to 0");

        RuleFor(u => u.Login)
            .NotEmpty().WithMessage("Login is required")
            .MaximumLength(50).WithMessage("Login cannot be longer than 50 characters");

        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email is required");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(3).WithMessage("Password must be at least 3 characters long")
            .MaximumLength(50).WithMessage("Password cannot be longer than 50 characters");

        RuleFor(u => u.AvatarUrl)
            .Must(BeAValidUrl).When(u => !string.IsNullOrEmpty(u.AvatarUrl))
            .WithMessage("Invalid Avatar URL");

        RuleFor(user => user.Roles)
             .NotNull().WithMessage("Roles cannot be null.")
             .Must(roles => roles != null && roles.Any())
             .WithMessage("User must have at least one role.");

        RuleForEach(u => u.FavoriteManga)
            .SetValidator(new FavoriteMangaValidator());
    }

    private bool BeAValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
