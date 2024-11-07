using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manga4ka.Api.Controllers;

[Route("/ratings")]
[ApiController]
public class RatingController(IRatingService ratingService) : ControllerBase
{
    private readonly IRatingService _service = ratingService;

    [HttpGet]
    public async Task<IActionResult> GetRating()
    {
        return Ok(await _service.GetAllRatingsAsync());
    }

    [HttpGet("average/{mangaId}")]
    public async Task<IActionResult> GetAverageRating(int mangaId)
    {
        return Ok(await _service.GetAverageRatingAsync(mangaId));
    }

    [Authorize]
    [HttpGet("userRate/{mangaId}/{userId}")]
    public async Task<IActionResult> IsUserRated(int mangaId, int userId)
    {
        return Ok(await _service.IsUserRated(mangaId, userId));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddRating([FromBody] RatingDto ratingDto)
    {
        await _service.AddRatingAsync(ratingDto);
        return Ok();
    }
}
