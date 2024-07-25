using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController(DataContext dataContext) : BaseApiController
{

    [HttpGet("auth")]
    public ActionResult<AppUser> GetAuth()
    {
        var things = dataContext.Users.Find(-1);
        if (things == null) return Unauthorized("Your credentials are invaild");
        return things;
    }
    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var things = dataContext.Users.Find(-1);
        if (things == null) return NotFound();
        return things;
    }
    [HttpGet("server-error")]
    public ActionResult<AppUser> GetServerError()
    {
        var things = dataContext.Users.Find(-1) ?? throw new Exception("something goes wrong");
        return things;
    }
    [HttpGet("bad-request")]
    public ActionResult<AppUser> GetBadRequest()
    {
        return BadRequest("This request not look correct");
    }
}