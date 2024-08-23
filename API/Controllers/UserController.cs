using API.Dtos;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[Authorize]
public class UserController : BaseApiController
{
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    private readonly IPhotoService _photoService;
    public UserController(IUserRepository _userRepository, IMapper _maper, IPhotoService photoService)
    {
        userRepository = _userRepository;
        mapper = _maper;
        _photoService = photoService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
    {
        userParams.CurrentUserName = User.GetUserName();
        var users = await userRepository.GetMembersAsync(userParams);
        if (users == null) return NotFound();
        Response.AddPaginationHeader(users);
        return Ok(users);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var users = await userRepository.GetUserByIdAsync(id);
        if (users == null) return NotFound();
        return Ok(users);
    }
    [HttpGet("GetUserByName")]
    public async Task<ActionResult<MemberDto>> GetUserByName(string name)
    {
        var users = await userRepository.GetMemberAsync(name);
        if (users == null) return NotFound();
        return Ok(users);
    }
    [HttpPut]
    public async Task<ActionResult<MemberDto>> UpdateUser(MemberUpdatedDto memeber)
    {
        var username = User.GetUserName();

        if (username is null) return BadRequest("No Username found in token!");

        var user = await userRepository.GetUserByNameAsync(username);

        if (user is null) return NotFound("Username is not found");
        mapper.Map(memeber, user);
        if (await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Fail to update user");
    }
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await userRepository.GetUserByNameAsync(User.GetUserName());
        if (user == null)
        {
            return BadRequest("Can't update user");
        }
        var result = await _photoService.AddPhotoAsync(file);
        if (result.Error != null)
        {
            return BadRequest(result.Error.Message);
        }
        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };
        if (user.Photos.Count == 0) photo.IsMain = true;
        user.Photos.Add(photo);
        if (await userRepository.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetUserByName), new { name = user.UserName }, mapper.Map<PhotoDto>(photo));
        }
        return BadRequest("Problem with adding photo");

    }
    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await userRepository.GetUserByNameAsync(User.GetUserName());
        if (user == null)
        {
            return BadRequest("Couldn't find user");
        }
        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null || photo.IsMain) return BadRequest("Can't use this as main photo");
        var currentmain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentmain != null) currentmain.IsMain = false;
        photo.IsMain = true;
        if (await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Problem setting main photo");
    }
    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await userRepository.GetUserByNameAsync(User.GetUserName());
        if (user == null)
        {
            return BadRequest("Couldn't find user");
        }
        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null || photo.IsMain) return BadRequest("This photo can't be deleted");
        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }
        user.Photos.Remove(photo);
        if (await userRepository.SaveAllAsync())
        {
            return Ok();
        }

        return BadRequest("Something unexpected happend");

    }
}