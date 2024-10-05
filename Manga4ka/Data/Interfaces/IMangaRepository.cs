namespace Manga4ka.Data.Interfaces;
using Manga4ka.Data.Entities;
public interface IMangaRepository : IRepository<Manga>
{
    public Task<IEnumerable<Manga>> SearchAsync(string query);
}
