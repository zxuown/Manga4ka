using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Manga4ka.Api.Controllers;

[Route("/account")]
[ApiController]
public class AccountController(IAccountService accountService) : ControllerBase
{
    private readonly IAccountService _accountService = accountService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        await _accountService.Register(registerDto);
        return StatusCode(201);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var jwtToken = await _accountService.Login(loginDto);
        return Ok(new
        {
            token = jwtToken,
        });
    }

    [Authorize]
    [HttpGet("currentUser")]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return Ok(await _accountService.GetUserByIdAsync(userId));
    }
}
