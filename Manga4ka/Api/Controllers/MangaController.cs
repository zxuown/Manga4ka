using Manga4ka.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Manga4ka.Business.Models;
using AutoMapper;
using Manga4ka.Business.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Manga4ka.Api.Controllers;

[ApiController]
[Route("/manga")]
public class MangaController(IMangaService mangaService) : ControllerBase
{
    private readonly IMangaService _service = mangaService;

    [HttpGet("search")]
    public async Task<ActionResult> SearchManga([FromQuery] string query)
    {
        return Ok(await _service.SearchMangaAsync(query));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MangaDto>>> GetManga()
    {
        return Ok(await _service.GetAllMangaAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MangaDto>> GetMangaById(int id)
    {
        return Ok(await _service.GetMangaByIdAsync(id));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> AddManga([FromBody] MangaDto mangaDto)
    {
        await _service.AddMangaAsync(mangaDto);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateManga([FromBody] MangaDto mangaDto)
    {
        await _service.UpdateMangaAsync(mangaDto);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteManga(int id)
    {
        await _service.DeleteMangaAsync(id);
        return Ok();
    }

    [HttpGet("sortMangaByPublishedAsc")]
    public async Task<ActionResult<IEnumerable<MangaDto>>> SortMangaByPublishedAsc()
    {
        return Ok(await _service.SortMangaByPublishedAscAsync());
    }

    [HttpGet("sortMangaByPublishedDesc")]
    public async Task<ActionResult<IEnumerable<MangaDto>>> SortMangaByPublishedDesc()
    {
        return Ok(await _service.SortMangaByPublishedDescAsync());
    }

    [HttpGet("sortMangaByRatingAsc")]
    public async Task<ActionResult<IEnumerable<MangaDto>>> SortMangaByRatingAsc()
    {
        return Ok(await _service.SortMangaByRatingAscAsync());
    }

    [HttpGet("sortMangaByRatingDesc")]
    public async Task<ActionResult<IEnumerable<MangaDto>>> SortMangaByRatingDesc()
    {
        return Ok(await _service.SortMangaByRatingDescAsync());
    }

    [HttpGet("sortMangaByFavorite/{userId}")]
    public async Task<ActionResult<IEnumerable<MangaDto>>> SortMangaByFavorite(int userId)
    {
        return Ok(await _service.SortMangaByFavoriteAsync(userId));
    }

    [HttpGet("favoriteManga")]
    [Authorize]
    public async Task<ActionResult> GetAllUserFavoriteManga()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return Ok(await _service.GetAllFavoriteMangaByUserIdAsync(userId));
    }

    [HttpPost("favoriteManga")]
    public async Task<ActionResult> AddFavoriteManga(FavoriteMangaDto favoriteMangaDto)
    {
        await _service.AddFavoriteMangaAsync(favoriteMangaDto);
        return Ok();
    }

    [HttpDelete("favoriteManga/{mangaId}/{userId}")]
    public async Task<ActionResult> DeleteFavoriteManga(int mangaId, int userId)
    {
        await _service.DeleteFavoriteMangaAsync(mangaId, userId);
        return Ok();
    }
}
