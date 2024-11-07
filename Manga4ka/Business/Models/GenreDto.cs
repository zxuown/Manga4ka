namespace Manga4ka.Business.Models;

public record GenreDto
{
    public int Id { get; init; }
    public string Title { get; init; }

    public string Description { get; init; }

    public string Image { get; init; }
}
