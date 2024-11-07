using AutoMapper;
using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Data.Entities;
using Manga4ka.Data.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace Manga4ka.Business.Services;

public class AccountService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : IAccountService
{
    private IUnitOfWork _unitOfWork { get; } = unitOfWork;

    private IMapper _mapper { get; } = mapper;

    private IConfiguration _config { get; } = configuration;
    public async Task Register(RegisterDto registerDto)
    {
        var user = _mapper.Map<User>(registerDto);
        user.Roles ??= new List<string>();
        user.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
        user.Roles.Add("User");
        await _unitOfWork.Users.Register(user);
        await _unitOfWork.SaveAsync();
    }

    public async Task<string> Login(LoginDto loginDto)
    {
        var user = await _unitOfWork.Users.Login(loginDto.LoginOrEmail, loginDto.Password);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Tokens:Token"]);
        var claims = new List<Claim>
        {
             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
             new Claim(ClaimTypes.Name, user.Name),
        };

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public async Task<bool> UserExists(string loginOrEmail)
    {
        return await _unitOfWork.Users.UserExists(loginOrEmail);
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        return _mapper.Map<UserDto>(await _unitOfWork.Users.GetByIdAsync(id));
    }
}
