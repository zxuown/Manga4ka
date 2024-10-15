using System.ComponentModel.DataAnnotations.Schema;

namespace Manga4ka.Data.Entities;

public class Rating : BaseEntity
{
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public User User { get; set; }

    [ForeignKey(nameof(Manga))]
    public int MangaId { get; set; }
    public Manga Manga { get; set; }
    public int Value { get; set; }
}
