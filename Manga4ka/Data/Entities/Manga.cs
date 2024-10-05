using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manga4ka.Data.Entities;

public class Manga : BaseEntity
{
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DatePublished { get; set; }

    public virtual ICollection<MangaGenre> MangaGenres { get; set; }

    [ForeignKey(nameof(Author))]

    public int AuthorId { get; set; }
    public virtual Author Author { get; set; }

    public string Image {  get; set; }

    public string Pdfile { get; set; }
}
