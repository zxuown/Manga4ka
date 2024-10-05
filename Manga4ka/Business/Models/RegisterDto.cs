namespace Manga4ka.Business.Models;

public class RegisterDto
{
    public string Login { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public string? AvatarUrl { get; set; }
}
