using Manga4ka.Data.Entities;

namespace Manga4ka.Data.Interfaces;

public interface IRatingRepository : IRepository<Rating>
{
    Task<double> GetAverageRatingAsync(int mangaId);
    Task<bool> IsUserRated(int mangaId, int userId);
}
