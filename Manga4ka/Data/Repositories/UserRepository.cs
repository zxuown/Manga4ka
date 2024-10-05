using Manga4ka.Data.Entities;
using Manga4ka.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Manga4ka.Data.Repositories;

public class UserRepository(Manga4kaContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User> Login(string loginOrEmail, string password)
    {
        var user = await _entities.FirstOrDefaultAsync(user => user.Login == loginOrEmail || user.Email == loginOrEmail);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null;
        }

        return user;
    }

    public async Task<User> Register(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        await _entities.AddAsync(user);
        return user;
    }

    public async Task<bool> UserExists(string loginOrEmail)
    {
        return await _entities.AnyAsync(user => user.Login == loginOrEmail || user.Email == loginOrEmail);
    }
}
