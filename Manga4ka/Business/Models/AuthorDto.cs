namespace Manga4ka.Business.Models;

public record AuthorDto
{
    public int Id { get; init; }
    public string Name { get; init; }

    public string Lastname { get; init; }

    public string Description { get; init; }

    public DateTime DateOfBirth { get; init; }

    public string Image { get; init; }
}
