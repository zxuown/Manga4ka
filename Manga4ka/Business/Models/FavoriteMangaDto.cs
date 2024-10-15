using Manga4ka.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manga4ka.Business.Models;

public class FavoriteMangaDto
{
    public int Id { get; set; }
    public MangaDto Manga { get; set; }
    public UserDto User { get; set; }
    public bool IsFavorite { get; set; }
}
