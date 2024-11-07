using Manga4ka.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Manga4ka.Business.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Manga4ka.Api.Controllers;

[ApiController]
[Route("/manga")]
public class MangaController(IMangaService mangaService) : ControllerBase
{
    private readonly IMangaService _service = mangaService;

    [HttpGet("search")]
    public async Task<IActionResult> SearchManga([FromQuery] string query)
    {
        return Ok(await _service.SearchMangaAsync(query));
    }

    [HttpGet]
    public async Task<IActionResult> GetManga()
    {
        return Ok(await _service.GetAllMangaAsync());
    }

    [HttpGet("mangaGenres")]
    public async Task<IActionResult> GetMangaGenres()
    {
        return Ok(await _service.GetAllMangaGenresAsync());
    }

    [HttpGet("mangaGenres/{mangaId}")]
    public async Task<IActionResult> GetMangaGenresByMangaId(int mangaId)
    {
        return Ok(await _service.GetAllMangaGenresByMangaIdAsync(mangaId));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("mangaGenres")]
    public async Task<IActionResult> AddMangaGenres([FromBody] List<MangaGenreDto> mangaGenreDtos)
    {
        await _service.AddMangaGenresAsync(mangaGenreDtos);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("mangaGenres")]
    public async Task<IActionResult> DeleteMangaGenres([FromBody] List<MangaGenreDto> mangaGenreDtos)
    {
        await _service.DeleteMangaGenresAsync(mangaGenreDtos);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMangaById(int id)
    {
        return Ok(await _service.GetMangaByIdAsync(id));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddManga([FromBody] MangaDto mangaDto)
    {
        return Ok(await _service.AddMangaAsync(mangaDto));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateManga([FromBody] MangaDto mangaDto)
    {
        await _service.UpdateMangaAsync(mangaDto);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteManga(int id)
    {
        await _service.DeleteMangaAsync(id);
        return Ok();
    }

    [HttpGet("sortMangaByPublishedAsc")]
    public async Task<IActionResult> SortMangaByPublishedAsc()
    {
        return Ok(await _service.SortMangaByPublishedAscAsync());
    }

    [HttpGet("sortMangaByPublishedDesc")]
    public async Task<IActionResult> SortMangaByPublishedDesc()
    {
        return Ok(await _service.SortMangaByPublishedDescAsync());
    }

    [HttpGet("sortMangaByRatingAsc")]
    public async Task<IActionResult> SortMangaByRatingAsc()
    {
        return Ok(await _service.SortMangaByRatingAscAsync());
    }

    [HttpGet("sortMangaByRatingDesc")]
    public async Task<IActionResult> SortMangaByRatingDesc()
    {
        return Ok(await _service.SortMangaByRatingDescAsync());
    }

    [HttpGet("sortMangaByFavorite/{userId}")]
    public async Task<IActionResult> SortMangaByFavorite(int userId)
    {
        return Ok(await _service.SortMangaByFavoriteAsync(userId));
    }

    [HttpGet("favoriteManga")]
    [Authorize]
    public async Task<IActionResult> GetAllUserFavoriteManga()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return Ok(await _service.GetAllFavoriteMangaByUserIdAsync(userId));
    }

    [HttpPost("favoriteManga")]
    [Authorize]
    public async Task<IActionResult> AddFavoriteManga(FavoriteMangaDto favoriteMangaDto)
    {
        await _service.AddFavoriteMangaAsync(favoriteMangaDto);
        return Ok();
    }

    [HttpDelete("favoriteManga/{mangaId}/{userId}")]
    [Authorize]
    public async Task<IActionResult> DeleteFavoriteManga(int mangaId, int userId)
    {
        await _service.DeleteFavoriteMangaAsync(mangaId, userId);
        return Ok();
    }
}
