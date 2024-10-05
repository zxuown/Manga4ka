﻿using AutoMapper;
using Manga4ka.Business.Interfaces;
using Manga4ka.Data.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public async Task<MangaDto> GetMangaByIdAsync(int id)
    {
        Manga manga = await _unitOfWork.Manga.GetByIdAsync(id);
        var mangaDto = _mapper.Map<MangaDto>(manga);
        mangaDto.Genres = mangaDto.MangaGenres.Select(x => _mapper.Map<GenreDto>(x.Genre)).ToList();
        return mangaDto;
    }
    public async Task<IEnumerable<MangaDto>> GetAllMangaAsync()
    {
        IEnumerable<Manga> manga = await _unitOfWork.Manga.GetAllAsync();
        var mangaDtos = _mapper.Map<IEnumerable<MangaDto>>(manga);
        foreach (var item in mangaDtos)
        {
            item.Genres = item.MangaGenres.Select(x => _mapper.Map<GenreDto>(x.Genre)).ToList();
        }
        return mangaDtos;
    }
}
