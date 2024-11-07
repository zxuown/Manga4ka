using Manga4ka.Data.Entities;
using Manga4ka.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Manga4ka.Data.Repositories;

public class RatingRepository(Manga4kaContext context) : BaseRepository<Rating>(context), IRatingRepository
{
    public async Task<double> GetAverageRatingAsync(int mangaId)
    {
        return await _entities
                    .Where(x => x.MangaId == mangaId)
                    .Select(x => (double?)x.Value)
                    .AverageAsync() ?? 0;
    }


    public Task<bool> IsUserRated(int mangaId, int userId)
    {
        return _entities.AnyAsync(x => x.MangaId == mangaId && x.UserId == userId);
    }
}
