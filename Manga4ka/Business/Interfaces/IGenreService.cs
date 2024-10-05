using Manga4ka.Business.Models;

namespace Manga4ka.Business.Interfaces;

public interface IGenreService
{
    Task <IEnumerable<GenreDto>> SearchGenreAsync(string query);
    Task AddGenreAsync(GenreDto entity);
    Task UpdateGenreAsync(GenreDto entity);
    Task DeleteGenreAsync(int id);
    Task<IEnumerable<GenreDto>> GetAllGenresAsync();
    Task<GenreDto> GetGenreByIdAsync(int id);
}
