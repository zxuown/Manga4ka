using Manga4ka.Data.Entities;
using Manga4ka.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Manga4ka.Data.Repositories;

public class BaseRepository<T>(Manga4kaContext context) : IRepository<T> where T : BaseEntity
{
    protected Manga4kaContext _context { get; } = context;

    protected DbSet<T> _entities { get; } = context.Set<T>();

    public virtual async Task AddAsync(T entity)
    {
        await _context.AddAsync(entity);
    }

    public virtual async Task UpdateAsync(T entity)
    {
         _context.Update(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
        T entity = await GetByIdAsync(id);
        _context.Remove(entity);
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _entities.FirstOrDefaultAsync(x => x.Id == id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _entities.AsNoTracking().ToListAsync();
    }
}
