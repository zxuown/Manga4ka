using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manga4ka.Api.Controllers;

[Route("/comments")]
[ApiController]
public class CommentController(ICommentService commentService) : ControllerBase
{
    private readonly ICommentService _service = commentService;

    [HttpGet("mangaId/{mangaId}")]
    public async Task<IActionResult> GetCommnetByMangaId(int mangaId)
    {
        return Ok(await _service.GetAllCommentsByMangaIdAsync(mangaId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommnetById(int id)
    {
        return Ok(await _service.GetCommentByIdAsync(id));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddCommnet([FromBody] CommentDto comment)
    {
        await _service.AddCommentAsync(comment);
        return Ok();
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCommnet([FromBody] CommentDto comment)
    {
        await _service.UpdateCommentAsync(comment);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCommnet(int id)
    {
        await _service.DeleteCommentAsync(id);
        return Ok();
    }
}
