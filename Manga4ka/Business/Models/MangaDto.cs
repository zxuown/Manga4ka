using Manga4ka.Data.Entities;
using System.Text.Json.Serialization;

namespace Manga4ka.Business.Models;

public record MangaDto
{
    public int Id { get; init; }
    public string Title { get; init; }

    public string Description { get; init; }

    public DateTime DatePublished { get; init; }

    public AuthorDto Author { get; init; }

    public string Image { get; init; }

    public string Pdfile { get; init; }
}
