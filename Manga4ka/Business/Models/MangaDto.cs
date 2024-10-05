using Manga4ka.Data.Entities;
using System.Text.Json.Serialization;

namespace Manga4ka.Business.Models;

public class MangaDto
{
    public int Id { get; set; }
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DatePublished { get; set; }

    public List<GenreDto> Genres { get; set; }

    [JsonIgnore]

    public List<MangaGenre> ? MangaGenres { get; set; }

    public AuthorDto Author { get; set; }

    public string Image { get; set; }

    public string Pdfile { get; set; }
}
