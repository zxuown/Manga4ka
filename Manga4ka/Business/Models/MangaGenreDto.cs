using Manga4ka.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manga4ka.Business.Models;

public record MangaGenreDto
{
    public int Id { get; init; } 
    public MangaDto Manga { get; init; }
    public GenreDto Genre { get; init; }
}
