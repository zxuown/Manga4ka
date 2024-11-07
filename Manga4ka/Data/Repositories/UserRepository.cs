using Manga4ka.Business.Models;
using Manga4ka.Data.Entities;
using Manga4ka.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Manga4ka.Data.Repositories;

public class UserRepository(Manga4kaContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User> Login(string loginOrEmail, string password)
    {
        return await _entities.FirstOrDefaultAsync(user => user.Login == loginOrEmail || user.Email == loginOrEmail); ;
    }

    public async Task<User> Register(User user)
    {
        await _entities.AddAsync(user);
        return user;
    }

    public async Task<bool> UserExists(string loginOrEmail)
    {
        return await _entities.AnyAsync(user => user.Login == loginOrEmail || user.Email == loginOrEmail);
    }
}
