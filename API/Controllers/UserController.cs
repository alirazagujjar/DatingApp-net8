using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController:ControllerBase
{
    private readonly DataContext context;
    public UserController(DataContext _context)
    {
        context=_context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users =await context.Users.ToListAsync();
        if(users==null) return NotFound();
        return Ok(users);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var users = await context.Users.FindAsync(id);
        if(users==null) return NotFound();
        return Ok(users);
    }
}