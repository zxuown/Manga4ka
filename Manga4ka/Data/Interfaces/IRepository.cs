namespace Manga4ka.Data.Interfaces;
using Manga4ka.Data.Entities;
public interface IRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
}
