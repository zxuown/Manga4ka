using System.ComponentModel.DataAnnotations.Schema;

namespace Manga4ka.Data.Entities;

public class MangaGenre
{
    [ForeignKey(nameof(Manga))]
    public int MangaId { get; set; }

    [ForeignKey(nameof(Genre))]
    public int GenreId { get; set; }

    public virtual Manga Manga { get; set; }

    public virtual Genre Genre { get; set; }
}
