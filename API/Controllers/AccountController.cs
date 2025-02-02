using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Dtos;
using API.Entities;
using API.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext context;
    private readonly ITokenService _tokenServic;
    private readonly IMapper _maper;
    public AccountController(DataContext _context, ITokenService tokenService,IMapper mapper)
    {
        context = _context;
        _tokenServic = tokenService;
        _maper= mapper;
    }
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterUser register)
    {
        if (await CheckUserExist(register.UserName))
        {
            return BadRequest(new { Message = "User alreday exist", status = 0 });
        }
    
        using var hmac = new HMACSHA512();
        var user = _maper.Map<AppUser>(register);
        user.UserName = user.UserName.ToLower();
        user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(register.PassWord));
        user.PasswordSalt = hmac.Key;
        context.Users.Add(user);
        await context.SaveChangesAsync();
        LoginDto loginDto = new LoginDto()
        {
            UserName = user.UserName,
            PassWord = register.PassWord
        };
        var loginResult = await Login(loginDto);
        return  loginResult;
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto login)
    {
        var user = await context.Users
        .Include(a=>a.Photos)
            .FirstOrDefaultAsync(a => a.UserName.ToLower() == login.UserName.ToLower());
        if (user == null) return Unauthorized(new { status = 0, messgae = "UserName is incorrect" });
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.PassWord));
        for (int i = 0; i < ComputeHash.Length; i++)
        {
            if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized(new { status = 0, messgae = "Password incorrect" });

        }
        var userdto = new UserDto
        {
            UserName = user.UserName,
            KnownAs = user.KnownAs,
            Token = _tokenServic.CreateToken(user),
            PhotoUrl= user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
            Gender = user.Gender
        };
        return userdto;
    }
    private async Task<bool> CheckUserExist(string username)
    {
        return await context.Users.AnyAsync(a => a.UserName.ToLower() == username.ToLower());

    }
}