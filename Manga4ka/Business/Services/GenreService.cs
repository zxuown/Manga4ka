using AutoMapper;
using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Data.Entities;
using Manga4ka.Data.Interfaces;

namespace Manga4ka.Business.Services;

public class GenreService(IUnitOfWork unitOfWork, IMapper mapper) : IGenreService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<GenreDto>> SearchGenreAsync(string query)
    {
        var filteredGenres = await _unitOfWork.Genres.Search(query);
        return _mapper.Map<IEnumerable<GenreDto>>(filteredGenres);
    }
    public async Task AddGenreAsync(GenreDto genre)
    {
        await _unitOfWork.Genres.AddAsync(_mapper.Map<Genre>(genre));
        await _unitOfWork.SaveAsync();
    }
    public async Task UpdateGenreAsync(GenreDto genre)
    {
        await _unitOfWork.Genres.UpdateAsync(_mapper.Map<Genre>(genre));
        await _unitOfWork.SaveAsync();
    }
    public async Task DeleteGenreAsync(int id)
    {
        await _unitOfWork.Genres.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
    }
    public async Task<GenreDto> GetGenreByIdAsync(int id)
    {
        Genre genre = await _unitOfWork.Genres.GetByIdAsync(id);
        return _mapper.Map<GenreDto>(genre);
    }
    public async Task<IEnumerable<GenreDto>> GetAllGenresAsync()
    {
        IEnumerable<Genre> genres = await _unitOfWork.Genres.GetAllAsync();
        return _mapper.Map<IEnumerable<GenreDto>>(genres);
    }
}
