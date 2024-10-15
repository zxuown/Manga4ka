namespace Manga4ka.Data.Interfaces;
using Manga4ka.Data.Entities;
using Microsoft.EntityFrameworkCore;

public interface IMangaRepository : IRepository<Manga>
{
    Task<IEnumerable<Manga>> SearchAsync(string query);
    Task AddFavoriteMangaAsync(FavoriteManga favoriteManga);
    Task DeleteFavoriteMangaAsync(int mangaId, int userId);
    Task<IEnumerable<Manga>> SortByPublishedAscAsync();
    Task<IEnumerable<Manga>> SortByPublishedDescAsync();
    Task<IEnumerable<Manga>> SortByRatingAscAsync();
    Task<IEnumerable<Manga>> SortByRatingDescAsync();
    Task<IEnumerable<Manga>> SortByFavoriteAsync(int userId);
    Task<IEnumerable<FavoriteManga>> GetAllFavoriteMangaByUserIdAsync(int userId);
}
