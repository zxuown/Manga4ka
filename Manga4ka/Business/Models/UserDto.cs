namespace Manga4ka.Business.Models;

public record UserDto
{
    public int Id { get; init; }
    public string Login { get; init; }

    public string Name { get; init; }

    public string Email { get; init; }

    public string Password { get; init; }

    public string? AvatarUrl { get; init; }

    public List<string> Roles { get; init; }

    public List<FavoriteMangaDto> FavoriteManga { get; init; }
}
