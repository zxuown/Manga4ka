namespace Manga4ka.Data.Entities;

public class Genre : BaseEntity
{
    public string Title { get; set; }

    public string Description { get; set; }

    public string Image { get; set; }

    public virtual ICollection<MangaGenre> MangaGenres { get; set; }
}
