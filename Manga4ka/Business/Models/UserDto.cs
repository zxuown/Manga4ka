namespace Manga4ka.Business.Models;

public class UserDto
{
    public int Id { get; set; }
    public string Login { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string? AvatarUrl { get; set; }

    public List<string> Roles { get; set; }

    public List<FavoriteMangaDto> FavoriteManga { get; set; }
}
