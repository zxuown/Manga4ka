using FluentValidation;
using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Business.Services;

namespace Manga4ka.Business.Validation;

public class LoginValidator : AbstractValidator<LoginDto>
{
    private readonly IAccountService _accountService;
    public LoginValidator(IAccountService accountService)
    {
        _accountService = accountService;
        RuleFor(x => x.LoginOrEmail)
           .NotEmpty().WithMessage("Login or email is required");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(3).WithMessage("Password must be at least 3 characters long");

        RuleFor(x => x)
                .MustAsync(BeAValidToken).WithMessage("Invalid login or password");
    }

    private async Task<bool> BeAValidToken(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var jwtToken = await _accountService.Login(loginDto);
        return !string.IsNullOrEmpty(jwtToken);
    }
}
