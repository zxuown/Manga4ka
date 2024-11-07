namespace Manga4ka.Business.Models;

public record RegisterDto
{
    public string Login { get; init; }

    public string Name { get; init; }

    public string Email { get; init; }

    public string Password { get; init; }

    public string ConfirmPassword { get; init; }

    public string? AvatarUrl { get; init; }
}
