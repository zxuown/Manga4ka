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
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<MangaDto>> SearchMangaAsync(string query)
    {
        var fileteredManga = await _unitOfWork.Manga.SearchAsync(query);
        var mangaDtos = _mapper.Map<IEnumerable<MangaDto>>(fileteredManga);
        foreach (var item in mangaDtos)
        {
            item.Genres = item.MangaGenres.Select(x => _mapper.Map<GenreDto>(x.Genre)).ToList();
        }
        return mangaDtos;
    }
    public async Task AddMangaAsync(MangaDto mangaDto)
    {
        var manga = _mapper.Map<Manga>(mangaDto);
        manga.Author = await _unitOfWork.Authors.GetByIdAsync(mangaDto.Author.Id);

        foreach (var genreDto in mangaDto.Genres)
        {
            manga.MangaGenres.Add(new()
            {
                MangaId = manga.Id,
                GenreId = genreDto.Id,
            });
        }

        await _unitOfWork.Manga.AddAsync(manga);
        await _unitOfWork.SaveAsync();
    }
    public async Task UpdateMangaAsync(MangaDto mangaDto)
    {
        var existingManga = await _unitOfWork.Manga.GetByIdAsync(mangaDto.Id);
        if (existingManga == null)
        {
            throw new Exception("Manga not found.");
        }

        _mapper.Map(mangaDto, existingManga);

        foreach (var genreDto in mangaDto.Genres)
        {
            existingManga.MangaGenres.Add(new()
            {
                MangaId = existingManga.Id,
                GenreId = genreDto.Id,
            });
        }

        await _unitOfWork.Manga.UpdateAsync(existingManga);
        await _unitOfWork.SaveAsync();
    }
    public async Task DeleteMangaAsync(int id)
    {
        MangaDto manga = await GetMangaByIdAsync(id);
        await _unitOfWork.Manga.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
    }
    public async Task<IEnumerable<MangaDto>> SortMangaByPublishedAscAsync()
    {
        return await SetMangaDtosGenresAsync(await _unitOfWork.Manga.SortByPublishedAscAsync());
    }
    public async Task<IEnumerable<MangaDto>> SortMangaByPublishedDescAsync()
    {
        return await SetMangaDtosGenresAsync(await _unitOfWork.Manga.SortByPublishedDescAsync());
    }
    public async Task<IEnumerable<MangaDto>> SortMangaByRatingAscAsync()
    {
        return await SetMangaDtosGenresAsync(await _unitOfWork.Manga.SortByRatingAscAsync());
    }
    public async Task<IEnumerable<MangaDto>> SortMangaByRatingDescAsync()
    {
        return await SetMangaDtosGenresAsync(await _unitOfWork.Manga.SortByRatingDescAsync());
    }
    public async Task<IEnumerable<MangaDto>> SortMangaByFavoriteAsync(int userId)
    {
        return await SetMangaDtosGenresAsync(await _unitOfWork.Manga.SortByFavoriteAsync(userId));
    }
    public async Task<MangaDto> GetMangaByIdAsync(int id)
    {
        Manga manga = await _unitOfWork.Manga.GetByIdAsync(id);
        var mangaDto = _mapper.Map<MangaDto>(manga);
        mangaDto.Genres = mangaDto.MangaGenres.Select(x => _mapper.Map<GenreDto>(x.Genre)).ToList();
        return mangaDto;
    }
    public async Task<IEnumerable<MangaDto>> GetAllMangaAsync()
    {
        return await SetMangaDtosGenresAsync(await _unitOfWork.Manga.GetAllAsync());
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
    public async Task<IEnumerable<MangaDto>> SetMangaDtosGenresAsync(IEnumerable<Manga> manga)
    {
        var mangaDtos = _mapper.Map<IEnumerable<MangaDto>>(manga);
        foreach (var item in mangaDtos)
        {
            item.Genres = item.MangaGenres.Select(x => _mapper.Map<GenreDto>(x.Genre)).ToList();
        }
        return mangaDtos;
    }
}
