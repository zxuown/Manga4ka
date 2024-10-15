using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
namespace Manga4ka.Data.Entities;

public class User : BaseEntity
{
    public string Login { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string? AvatarUrl { get; set; }

    public List<string> Roles { get; set; } = new List<string>();

    public virtual ICollection<FavoriteManga> FavoriteManga { get; set; }
}
