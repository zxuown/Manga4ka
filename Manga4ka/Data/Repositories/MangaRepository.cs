namespace Manga4ka.Data.Repositories;
using Data.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Manga4ka.Business.Models;
using System.Collections.Generic;

public class MangaRepository(Manga4kaContext context) : BaseRepository<Manga>(context), IMangaRepository
{
    public async Task<IEnumerable<Manga>> SearchAsync(string query)
    {
        var manga = await GetAllAsync();
        return manga.Where(x => x.Title.ToLower().Contains(query.ToLower()));
    }
    public override async Task<IEnumerable<Manga>> GetAllAsync()
    {
        return await _entities.Include(x => x.Author).ToListAsync();
    }
    public async Task<IEnumerable<MangaGenre>> GetAllMangaGenresAsync()
    {
        return await _context.MangaGenres.Include(x => x.Manga).Include(x => x.Genre).ToListAsync();
    }
    public async Task<IEnumerable<MangaGenre>> GetAllMangaGenresByMangaIdAsync(int mangaId)
    {
        var mangaGernes = _context.MangaGenres.ToList();
        var res = await _context.MangaGenres.Include(x => x.Manga).ThenInclude(x => x.Author).Include(x => x.Genre).Where(x => x.MangaId == mangaId).ToListAsync();
        return await _context.MangaGenres.Include(x => x.Manga).ThenInclude(x => x.Author).Include(x => x.Genre).Where(x => x.MangaId == mangaId).ToListAsync();
    }
    public async Task AddMangaGenreAsync(MangaGenre mangaGenre)
    {
        await _context.MangaGenres.AddAsync(mangaGenre);
    }
    public async Task DeleteMangaGenreAsync(int id)
    {
        _context.MangaGenres.Remove(await _context.MangaGenres.FirstOrDefaultAsync(x => x.Id == id));
    }
    public override async Task<Manga> GetByIdAsync(int id)
    {
        return await _entities.Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task AddFavoriteMangaAsync(FavoriteManga favoriteManga)
    {
        await _context.FavoriteManga.AddAsync(favoriteManga);
    }
    public async Task DeleteFavoriteMangaAsync(int mangaId, int userId)
    {
        var mangaList = await _context.FavoriteManga.Where(x => x.MangaId == mangaId && x.UserId == userId).ToListAsync();
        foreach (var item in mangaList)
        {
            _context.FavoriteManga.Remove(item);
        }
    }
    public async Task<IEnumerable<Manga>> SortByPublishedAscAsync()
    {
        return await _entities.Include(x => x.Author).OrderBy(x => x.DatePublished).ToListAsync();
    }
    public async Task<IEnumerable<Manga>> SortByPublishedDescAsync()
    {
        return await _entities.Include(x => x.Author).OrderByDescending(x => x.DatePublished).ToListAsync();
    }
    public async Task<IEnumerable<Manga>> SortByRatingAscAsync()
    {
        return await SortByRatingAsync(ascending: true);
    }
    public async Task<IEnumerable<Manga>> SortByRatingDescAsync()
    {
        return await SortByRatingAsync(ascending: false);
    }
    public async Task<IEnumerable<Manga>> SortByRatingAsync(bool ascending)
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
