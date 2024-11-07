using AutoMapper;
using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Data.Entities;
using Manga4ka.Data.Interfaces;

namespace Manga4ka.Business.Services;

public class RatingService(IUnitOfWork unitOfWork, IMapper mapper) : IRatingService
{
    private IUnitOfWork _unitOfWork { get; } = unitOfWork;

    private IMapper _mapper { get; } = mapper;
    public async Task AddRatingAsync(RatingDto entity)
    {
        var rating = _mapper.Map<Rating>(entity);
        rating.User = await _unitOfWork.Users.GetByIdAsync(entity.User.Id);
        rating.Manga = await _unitOfWork.Manga.GetByIdAsync(entity.Manga.Id);
        await _unitOfWork.Rating.AddAsync(rating);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<RatingDto>> GetAllRatingsAsync()
    {
        return _mapper.Map<IEnumerable<RatingDto>>(await _unitOfWork.Rating.GetAllAsync());
    }

    public  Task<double> GetAverageRatingAsync(int mangaId)
    {
        return  _unitOfWork.Rating.GetAverageRatingAsync(mangaId);
    }

    public async Task<bool> IsUserRated(int mangaId, int userId)
    {
        return await _unitOfWork.Rating.IsUserRated(mangaId, userId);
    }
}
