namespace Manga4ka.Data.Repositories;
using Data.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Manga4ka.Business.Models;
using System.Collections.Generic;
using Manga4ka.Migrations;

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
        return await _entities.Include(x => x.Author)
            .Include(x => x.MangaGenres)
            .ThenInclude(x => x.Genre)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task AddFavoriteMangaAsync(FavoriteManga favoriteManga)
    {
        await _context.FavoriteManga.AddAsync(favoriteManga);
    }
    public async Task DeleteFavoriteMangaAsync(int mangaId, int userId)
    {
        var favoriteManga = _context.FavoriteManga.Where(x => x.MangaId == mangaId && x.UserId == userId);
        foreach (var item in favoriteManga)
        {
            _context.FavoriteManga.Remove(item);
        }
    }
    public async Task<IEnumerable<Manga>> SortByPublishedAscAsync()
    {
        var manga = await GetAllAsync();
        return manga.OrderBy(x => x.DatePublished);
    }
    public async Task<IEnumerable<Manga>> SortByPublishedDescAsync()
    {
        var manga = await GetAllAsync();
        return manga.OrderByDescending(x => x.DatePublished);
    }
    public async Task<IEnumerable<Manga>> SortByRatingAscAsync()
    {
        return await SortByRatingAsync(ascending: true);
    }
    public async Task<IEnumerable<Manga>> SortByRatingDescAsync()
    {
        return await SortByRatingAsync(ascending: false);
    }
    private async Task<IEnumerable<Manga>> SortByRatingAsync(bool ascending)
    {
        var mangaWithRatings = await _context.Ratings
            .GroupBy(r => r.MangaId)
            .Select(g => new
            {
                MangaId = g.Key,
                AverageRating = g.Average(r => r.Value)
            })
            .OrderBy(x => ascending ? x.AverageRating : -x.AverageRating)
            .ToListAsync();

        var mangaIds = mangaWithRatings.Select(m => m.MangaId).ToList();

        var sortedManga = await _context.Manga
            .Where(m => mangaIds.Contains(m.Id))
            .Include(m => m.Author)
            .Include(m => m.MangaGenres)
            .ThenInclude(g => g.Genre)
            .ToListAsync();

        var mangaDict = sortedManga.ToDictionary(m => m.Id); 
        return mangaWithRatings
            .Select(x => mangaDict[x.MangaId])
            .ToList();
    }
    public async Task<IEnumerable<Manga>> SortByFavoriteAsync(int userId)
    {
        return await _context.FavoriteManga
            .Include(m => m.Manga)
            .ThenInclude(m => m.Author)
            .Include(m => m.Manga.MangaGenres)
            .ThenInclude(g => g.Genre)
            .Where(m => m.IsFavorite && m.UserId == userId)
            .Select(x => x.Manga)
            .ToListAsync();
    }
    public async Task<IEnumerable<FavoriteManga>> GetAllFavoriteMangaByUserIdAsync(int userId)
    {
        return await _context.FavoriteManga
            .Include(x => x.Manga)
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }
}
