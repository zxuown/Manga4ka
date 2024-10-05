using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Manga4ka.Api.Controllers;

[ApiController]
[Route("/genres")]
public class GenreController(IGenreService genreService) : ControllerBase
{
    private readonly IGenreService _service = genreService;

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<GenreDto>>> SearchGenre([FromQuery]string query)
    {
        return Ok(await _service.SearchGenreAsync(query));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenre()
    {
        return Ok(await _service.GetAllGenresAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GenreDto>> GetGenreById(int id)
    {
        return Ok(await _service.GetGenreByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult> AddGenre([FromBody]GenreDto genre)
    {
        await _service.AddGenreAsync(genre);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGenre([FromBody]GenreDto genre)
    {
        await _service.UpdateGenreAsync(genre);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGenre(int id)
    {
        await _service.DeleteGenreAsync(id);
        return Ok();
    }
}
