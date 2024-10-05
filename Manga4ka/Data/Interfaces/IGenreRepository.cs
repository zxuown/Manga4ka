namespace Manga4ka.Data.Interfaces;
using Manga4ka.Data.Entities;
public interface IGenreRepository : IRepository<Genre>
{
    Task<IEnumerable<Genre>> Search(string query);
}
