namespace Manga4ka.Business.Interfaces;
using Business.Models;
using Manga4ka.Data.Entities;

public interface IMangaService
{
    Task<IEnumerable<MangaDto>> SearchMangaAsync(string query);
    Task<int> AddMangaAsync(MangaDto entity);
    Task UpdateMangaAsync(MangaDto entity);
    Task DeleteMangaAsync(int id);
    Task<IEnumerable<MangaDto>> SortMangaByPublishedAscAsync();
    Task<IEnumerable<MangaDto>> SortMangaByPublishedDescAsync();
    Task<IEnumerable<MangaDto>> SortMangaByRatingAscAsync();
    Task<IEnumerable<MangaDto>> SortMangaByRatingDescAsync();
    Task<IEnumerable<MangaDto>> SortMangaByFavoriteAsync(int userId);
    Task<IEnumerable<MangaGenreDto>> GetAllMangaGenresAsync();
    Task<IEnumerable<MangaGenreDto>> GetAllMangaGenresByMangaIdAsync(int mangaId);
    Task AddMangaGenresAsync(List<MangaGenreDto> mangaGenreDtos);
    Task DeleteMangaGenresAsync(List<MangaGenreDto> mangaGenreDtos);
    Task<IEnumerable<MangaDto>> GetAllMangaAsync();
    Task<MangaDto> GetMangaByIdAsync(int id);
    Task AddFavoriteMangaAsync(FavoriteMangaDto favoriteMangaDto);
    Task DeleteFavoriteMangaAsync(int mangaId, int userId);
    Task<IEnumerable<FavoriteMangaDto>> GetAllFavoriteMangaByUserIdAsync(int userId);
}
