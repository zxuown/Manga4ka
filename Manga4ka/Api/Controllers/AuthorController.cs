using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manga4ka.Api.Controllers;

[ApiController]
[Route("/authors")]
public class AuthorController(IAuthorService authorService) : ControllerBase
{
    private readonly IAuthorService _service = authorService;

    [HttpGet("search")]
    public async Task<ActionResult> SearchAuthor([FromQuery] string query)
    {
        return Ok(await _service.SearchAuthorsAsync(query));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthor()
    {
        return Ok(await _service.GetAllAuthorsAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorDto>> GetAuthorById(int id)
    {
        return Ok(await _service.GetAuthorByIdAsync(id));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> AddAuthor([FromBody] AuthorDto author)
    {
        await _service.AddAuthorAsync(author);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAUthor([FromBody] AuthorDto author)
    {
        await _service.UpdateAuthorAsync(author);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuthor(int id)
    {
        await _service.DeleteAuthorAsync(id);
        return Ok();
    }
}
