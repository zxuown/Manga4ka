namespace Manga4ka.Data.Repositories;
using Data.Interfaces;
using Data.Entities;
public class GenreRepository(Manga4kaContext context) : BaseRepository<Genre>(context), IGenreRepository
{
    public async Task<IEnumerable<Genre>> Search(string query)
    {
        var genres = await GetAllAsync();
        return genres.Where(genre => genre.Title.ToLower().Contains(query.ToLower()));
    }
}
