namespace Manga4ka.Business.Models;

public record LoginDto
{
    public string LoginOrEmail { get; init; }

    public string Password { get; init; }
}
