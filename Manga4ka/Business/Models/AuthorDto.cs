namespace Manga4ka.Business.Models;

public class AuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Lastname { get; set; }

    public string Description { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Image { get; set; }
}
