using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Data.Entities;
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
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        if (registerDto.Password != registerDto.ConfirmPassword)
        {
            return BadRequest("Passwords don't match");
        }

        if (await _accountService.UserExists(registerDto.Login) || await _accountService.UserExists(registerDto.Email))
        {
            return BadRequest("User already exists");
        }

        await _accountService.Register(registerDto);
        return StatusCode(201);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        var jwtToken = await _accountService.Login(loginDto);
        if (jwtToken == null)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            token = jwtToken,
        });
    }

    [Authorize]
    [HttpGet("currentUser")]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(ClaimTypes.Name)?.Value;

        return Ok(new { userId, username });
    }
}
