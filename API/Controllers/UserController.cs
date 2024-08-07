using System.Security.Claims;
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
    private readonly IMapper mapper;
    public UserController(IUserRepository _userRepository,IMapper _maper)
    {
        userRepository=_userRepository;
        mapper = _maper;
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
    [HttpPut]
    public async Task<ActionResult<MemberDto>> UpdateUser(MemberUpdatedDto memeber)
    {
        var username =  User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(username  is null) return BadRequest("No Username found in token!");

        var user = await userRepository.GetUserByNameAsync(username);

        if(user is null) return NotFound("Username is not found");
        mapper.Map(memeber,user);
        if(await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Fail to update user");
    }
}