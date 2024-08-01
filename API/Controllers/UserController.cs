using API.Data;
using API.Dtos;
using API.Entities;
using API.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;

[Authorize]
public class UserController:BaseApiController
{
    private readonly IUserRepository userRepository;
    public UserController(IUserRepository _userRepository)
    {
        userRepository=_userRepository;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users =await userRepository.GetMembersAsync();
        if(users==null) return NotFound();
        return Ok(users);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var users = await userRepository.GetUserByIdAsync(id);
        if(users==null) return NotFound();
        return Ok(users);
    }
    [HttpGet("GetUserByName")]
    public async Task<ActionResult<MemberDto>> GetUserByName(string name)
    {
        var users = await userRepository.GetMemberAsync(name);
        if(users==null) return NotFound();
        return Ok(users);
    }
}