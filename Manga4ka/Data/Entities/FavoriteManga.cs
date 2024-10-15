using System.ComponentModel.DataAnnotations.Schema;

namespace Manga4ka.Data.Entities;

public class FavoriteManga : BaseEntity
{
    [ForeignKey(nameof(Manga))]
    public int MangaId { get; set; }
    public Manga Manga { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public User User { get; set; }

    public bool IsFavorite { get; set; }
}
