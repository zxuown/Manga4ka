using AutoMapper;
using Manga4ka.Business.Interfaces;
using Manga4ka.Data.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace Manga4ka.Business.Services;

public class MangaService(IUnitOfWork unitOfWork, IMapper mapper) : IMangaService
{
    private IUnitOfWork _unitOfWork { get; } = unitOfWork;

    private IMapper _mapper { get; } = mapper;

    public async Task<IEnumerable<MangaDto>> SearchMangaAsync(string query)
    {
        var fileteredManga = await _unitOfWork.Manga.SearchAsync(query);
        var mangaDtos = _mapper.Map<IEnumerable<MangaDto>>(fileteredManga);
        //foreach (var item in mangaDtos)
        //{
        //    item.Genres = item.MangaGenres.Select(x => _mapper.Map<GenreDto>(x.Genre)).ToList();
        //}
        return mangaDtos;
    }
    public async Task<int> AddMangaAsync(MangaDto mangaDto)
    {
        var manga = _mapper.Map<Manga>(mangaDto);
        manga.Author = await _unitOfWork.Authors.GetByIdAsync(mangaDto.Author.Id);
        await _unitOfWork.Manga.AddAsync(manga);
        await _unitOfWork.SaveAsync();
        return manga.Id;
    }
    public async Task UpdateMangaAsync(MangaDto mangaDto)
    {
        var existingManga = await _unitOfWork.Manga.GetByIdAsync(mangaDto.Id);
        _mapper.Map(mangaDto, existingManga);
        await _unitOfWork.Manga.UpdateAsync(existingManga);
        await _unitOfWork.SaveAsync();
    }
    public async Task DeleteMangaAsync(int id)
    {
        await _unitOfWork.Manga.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
    }
    public async Task<IEnumerable<MangaDto>> SortMangaByPublishedAscAsync()
    {
        return _mapper.Map<IEnumerable<MangaDto>>(await _unitOfWork.Manga.SortByPublishedAscAsync());
    }
    public async Task<IEnumerable<MangaDto>> SortMangaByPublishedDescAsync()
    {
        return _mapper.Map<IEnumerable<MangaDto>>(await _unitOfWork.Manga.SortByPublishedDescAsync());
    }
    public async Task<IEnumerable<MangaDto>> SortMangaByRatingAscAsync()
    {
        return _mapper.Map<IEnumerable<MangaDto>>(await _unitOfWork.Manga.SortByRatingAscAsync());
    }
    public async Task<IEnumerable<MangaDto>> SortMangaByRatingDescAsync()
    {
        return _mapper.Map<IEnumerable<MangaDto>>(await _unitOfWork.Manga.SortByRatingDescAsync());
    }
    public async Task<IEnumerable<MangaDto>> SortMangaByFavoriteAsync(int userId)
    {
        return _mapper.Map<IEnumerable<MangaDto>>(await _unitOfWork.Manga.SortByFavoriteAsync(userId));
    }
    public async Task<MangaDto> GetMangaByIdAsync(int id)
    {
        Manga manga = await _unitOfWork.Manga.GetByIdAsync(id);
        var mangaDto = _mapper.Map<MangaDto>(manga);
        return mangaDto;
    }
    public async Task<IEnumerable<MangaDto>> GetAllMangaAsync()
    {
        var test = _mapper.Map<IEnumerable<MangaDto>>(await _unitOfWork.Manga.GetAllAsync());
        return _mapper.Map<IEnumerable<MangaDto>>(await _unitOfWork.Manga.GetAllAsync());
    }
    public async Task<IEnumerable<MangaGenreDto>> GetAllMangaGenresAsync()
    {
        return _mapper.Map<IEnumerable<MangaGenreDto>>(await _unitOfWork.Manga.GetAllMangaGenresAsync());
    }
    public async Task<IEnumerable<MangaGenreDto>> GetAllMangaGenresByMangaIdAsync(int mangaId)
    {
        return _mapper.Map<IEnumerable<MangaGenreDto>>(await _unitOfWork.Manga.GetAllMangaGenresByMangaIdAsync(mangaId));
    }
    public async Task AddMangaGenresAsync(List<MangaGenreDto> mangaGenreDtos)
    {
        foreach (var item in mangaGenreDtos)
        {
            var mangaGenre = _mapper.Map<MangaGenre>(item);
            mangaGenre.Manga = await _unitOfWork.Manga.GetByIdAsync(item.Manga.Id);
            mangaGenre.Genre = await _unitOfWork.Genres.GetByIdAsync(item.Genre.Id);
            await _unitOfWork.Manga.AddMangaGenreAsync(mangaGenre);
        }

        await _unitOfWork.SaveAsync();
    }
    public async Task DeleteMangaGenresAsync(List<MangaGenreDto> mangaGenreDtos)
    {
        foreach (var item in mangaGenreDtos)
        {
            await _unitOfWork.Manga.DeleteMangaGenreAsync(item.Id);
        }
        await _unitOfWork.SaveAsync();
    }
    public async Task AddFavoriteMangaAsync(FavoriteMangaDto favoriteMangaDto)
    {
        var favoriteManga = _mapper.Map<FavoriteManga>(favoriteMangaDto);
        favoriteManga.User = await _unitOfWork.Users.GetByIdAsync(favoriteMangaDto.User.Id);
        favoriteManga.Manga = await _unitOfWork.Manga.GetByIdAsync(favoriteMangaDto.Manga.Id);
        await _unitOfWork.Manga.AddFavoriteMangaAsync(favoriteManga);
        await _unitOfWork.SaveAsync();
    }
    public async Task DeleteFavoriteMangaAsync(int mangaId, int userId)
    {
        await _unitOfWork.Manga.DeleteFavoriteMangaAsync(mangaId, userId);
        await _unitOfWork.SaveAsync();
    }
    public async Task<IEnumerable<FavoriteMangaDto>> GetAllFavoriteMangaByUserIdAsync(int userId)
    {
        return _mapper.Map<IEnumerable<FavoriteMangaDto>>(await _unitOfWork.Manga.GetAllFavoriteMangaByUserIdAsync(userId));
    }
}
