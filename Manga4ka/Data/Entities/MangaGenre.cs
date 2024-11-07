using System.ComponentModel.DataAnnotations.Schema;

namespace Manga4ka.Data.Entities;

public class MangaGenre : BaseEntity
{
    [ForeignKey(nameof(Manga))]
    public int MangaId { get; set; }

    [ForeignKey(nameof(Genre))]
    public int GenreId { get; set; }

    public Manga Manga { get; set; }

    public Genre Genre { get; set; }
}
