using Manga4ka.Business.Models;
using Manga4ka.Data.Entities;

namespace Manga4ka.Business.Interfaces;

public interface IAccountService
{
    Task Register(RegisterDto registerDto);

    Task<string> Login(LoginDto registerDto);

    Task<bool> UserExists(string loginOrEmail);
}
