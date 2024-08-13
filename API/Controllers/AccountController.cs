using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Dtos;
using API.Entities;
using API.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext context;
    private readonly ITokenService _tokenServic;
    public AccountController(DataContext _context, ITokenService tokenService)
    {
        context = _context;
        _tokenServic = tokenService;
    }
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterUser register)
    {
        if (await CheckUserExist(register.UserName))
        {
            return BadRequest(new { Message = "User alreday exist", status = 0 });
        }
        return Ok();
        // using var hmac = new HMACSHA512();
        // var user = new AppUser
        // {
        //     UserName = register.UserName.ToLower(),
        //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.PassWord)),
        //     PasswordSalt = hmac.Key
        // };
        // context.Users.Add(user);
        // await context.SaveChangesAsync();
        // LoginDto loginDto = new LoginDto()
        // {
        //     UserName = user.UserName,
        //     PassWord = register.PassWord
        // };
        // var loginResult = await Login(loginDto);
        // return  loginResult;
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
            Token = _tokenServic.CreateToken(user),
            PhotoUrl= user.Photos.FirstOrDefault(x=>x.IsMain)?.Url
        };
        return userdto;
    }
    private async Task<bool> CheckUserExist(string username)
    {
        return await context.Users.AnyAsync(a => a.UserName.ToLower() == username.ToLower());

    }
}