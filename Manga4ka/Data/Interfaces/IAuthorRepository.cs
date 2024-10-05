using Manga4ka.Data.Entities;

namespace Manga4ka.Data.Interfaces;

public interface IAuthorRepository : IRepository<Author>
{
    Task<IEnumerable<Author>> Search(string query);
}
