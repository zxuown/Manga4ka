using Manga4ka.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Manga4ka.Business.Models;
using AutoMapper;
using Manga4ka.Business.Services;

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

    [HttpPost]
    public async Task<ActionResult> AddManga([FromBody] MangaDto mangaDto)
    {
        await _service.AddMangaAsync(mangaDto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateManga([FromBody] MangaDto mangaDto)
    {
        await _service.UpdateMangaAsync(mangaDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteManga(int id)
    {
        await _service.DeleteMangaAsync(id);
        return Ok();
    }
}
