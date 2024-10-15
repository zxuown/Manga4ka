using Manga4ka.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manga4ka.Business.Models;

public class CommentDto
{
    public int Id { get; set; }
    public UserDto User { get; set; }
    public MangaDto Manga { get; set; }
    public string Text { get; set; }
    public DateTime DatePublished { get; set; }
}
