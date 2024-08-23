using System.Security.Claims;

namespace API.Extensions;

public static class ClaimPrincipalExtensions
{
    public static string GetUserName(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.Name) 
        ?? throw new Exception("Can't get username from token");
        return username;

    }
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) 
        ?? throw new Exception("User Id not found"));

        return userId;

    }
}