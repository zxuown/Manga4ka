namespace Manga4ka.Business.Interfaces;
using Business.Models;
public interface IMangaService
{
    Task<IEnumerable<MangaDto>> SearchMangaAsync(string query);
    Task AddMangaAsync(MangaDto entity);
    Task UpdateMangaAsync(MangaDto entity);
    Task DeleteMangaAsync(int id);
    Task<IEnumerable<MangaDto>> GetAllMangaAsync();
    Task<MangaDto> GetMangaByIdAsync(int id);
}
