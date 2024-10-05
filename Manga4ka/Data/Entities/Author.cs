namespace Manga4ka.Data.Entities;

public class Author : BaseEntity
{
    public string Name { get; set; }

    public string Lastname { get; set; }

    public string Description { get; set; }

    public DateTime DateOfBirth {  get; set; }

    public string Image {  get; set; }  
}
