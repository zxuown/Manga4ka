using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manga4ka.Api.Controllers;

[ApiController]
[Route("/authors")]
public class AuthorController(IAuthorService authorService) : ControllerBase
{
    private readonly IAuthorService _service = authorService;

    [HttpGet("search")]
    public async Task<IActionResult> SearchAuthor([FromQuery] string query)
    {
        return Ok(await _service.SearchAuthorsAsync(query));
    }

    [HttpGet]
    public async Task<IActionResult> GetAuthor()
    {
        return Ok(await _service.GetAllAuthorsAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthorById(int id)
    {
        return Ok(await _service.GetAuthorByIdAsync(id));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddAuthor([FromBody] AuthorDto author)
    {
        await _service.AddAuthorAsync(author);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAUthor([FromBody] AuthorDto author)
    {
        await _service.UpdateAuthorAsync(author);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        await _service.DeleteAuthorAsync(id);
        return Ok();
    }
}
