using Manga4ka.Business.Models;

namespace Manga4ka.Business.Interfaces;

public interface IRatingService
{
    Task AddRatingAsync(RatingDto entity);
    Task<IEnumerable<RatingDto>> GetAllRatingsAsync();
    Task<double> GetAverageRatingAsync(int mangaId);
    Task<bool> IsUserRated(int mangaId, int userId);
}
