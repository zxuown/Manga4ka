﻿using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manga4ka.Api.Controllers;

[Route("/comments")]
[ApiController]
public class CommentController(ICommentService commentService) : ControllerBase
{
    private readonly ICommentService _service = commentService;

    [HttpGet("mangaId/{mangaId}")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommnetByMangaId(int mangaId)
    {
        return Ok(await _service.GetAllCommentsByMangaIdAsync(mangaId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetCommnetById(int id)
    {
        return Ok(await _service.GetCommentByIdAsync(id));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> AddCommnet([FromBody] CommentDto comment)
    {
        await _service.AddCommentAsync(comment);
        return Ok();
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCommnet([FromBody] CommentDto comment)
    {
        await _service.UpdateCommentAsync(comment);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCommnet(int id)
    {
        await _service.DeleteCommentAsync(id);
        return Ok();
    }
}