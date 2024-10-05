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
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private readonly IMapper _mapper = mapper;

    private readonly IConfiguration _config = configuration;
    public async Task Register(RegisterDto registerDto)
    {
        await _unitOfWork.Users.Register(_mapper.Map<User>(registerDto));
        await _unitOfWork.SaveAsync();
    }

    public async Task<string> Login(LoginDto loginDto)
    {
        var user = await _unitOfWork.Users.Login(loginDto.LoginOrEmail, loginDto.Password);
        if (user == null)
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Tokens:Token"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new []
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
            }),
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
}
