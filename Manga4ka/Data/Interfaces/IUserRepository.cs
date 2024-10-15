using Manga4ka.Business.Models;
using Manga4ka.Data.Entities;
using Manga4ka.Data.Repositories;

namespace Manga4ka.Data.Interfaces;

public interface IUserRepository : IRepository<User> 
{
    Task<User> Register(User user);
    Task<User> Login(string loginOrEmail, string password);
    Task<bool> UserExists(string loginOrEmail);
}
