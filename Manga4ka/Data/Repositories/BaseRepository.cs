using Manga4ka.Data.Entities;
using Manga4ka.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Manga4ka.Data.Repositories;

public class BaseRepository<T>(Manga4kaContext context) : IRepository<T> where T : BaseEntity
{
    protected readonly Manga4kaContext _context = context;

    protected readonly DbSet<T> _entities = context.Set<T>();

    public async Task AddAsync(T entity)
    {
        await _context.AddAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
         _context.Update(entity);
    }

    public async Task DeleteAsync(int id)
    {
        T entity = await GetByIdAsync(id);
        _context.Remove(entity);
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _entities.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _entities.AsNoTracking().ToListAsync();
    }
}
