namespace Manga4ka.Data.Repositories;
using Data.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Manga4ka.Business.Models;

public class MangaRepository(Manga4kaContext context) : BaseRepository<Manga>(context), IMangaRepository
{
    public async Task<IEnumerable<Manga>> SearchAsync(string query)
    {
        var manga = await GetAllAsync();
        return manga.Where(x => x.Title.ToLower().Contains(query.ToLower()));
    }
    public async new Task<IEnumerable<Manga>> GetAllAsync()
    {
        return await _entities.Include(x => x.Author).Include(x => x.MangaGenres).ThenInclude(x => x.Genre).ToListAsync();
    }
    public async new Task<Manga> GetByIdAsync(int id)
    {
        return await _entities.Include(x => x.Author).Include(x => x.MangaGenres).ThenInclude(x => x.Genre).FirstOrDefaultAsync(x => x.Id == id);
    }
}
