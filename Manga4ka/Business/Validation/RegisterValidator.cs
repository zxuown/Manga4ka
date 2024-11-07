using FluentValidation;
using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;

namespace Manga4ka.Business.Validation;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    private readonly IAccountService _accountService;
    public RegisterValidator(IAccountService accountService)
    {
        _accountService = accountService;

        RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Login is required")
                .MaximumLength(50).WithMessage("Login cannot be longer than 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email address is required");

        RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(3).WithMessage("Password must be at least 3 characters long")
                .MaximumLength(50).WithMessage("Password cannot be longer than 50 characters");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required")
            .Equal(x => x.Password).WithMessage("Passwords do not match");

        RuleFor(x => x.AvatarUrl)
            .Must(BeAValidUrl).When(x => !string.IsNullOrEmpty(x.AvatarUrl))
            .WithMessage("Invalid Avatar URL");

        RuleFor(x => x)
           .MustAsync(BeAValidUser).WithMessage("User with this login or email already exists");
    }

    private bool BeAValidUrl(string? avatarUrl)
    {
        return Uri.TryCreate(avatarUrl, UriKind.Absolute, out _);
    }

    private async Task<bool> BeAValidUser(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        var userExists = await _accountService.UserExists(registerDto.Login)
                         || await _accountService.UserExists(registerDto.Email);
        return !userExists; 
    }
}
