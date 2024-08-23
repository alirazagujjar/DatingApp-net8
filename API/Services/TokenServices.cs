using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Entities;
using API.interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Service;
public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    public TokenService(IConfiguration configuration)
    {
        _config=configuration;
    }
    public string CreateToken(AppUser appUser)
    {
        var tokenkey = _config["TokenKey"] ?? throw new Exception("Can't get Tokenkey from configuration file");
        if(tokenkey.Length<64) throw new Exception("Token key is too short");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey));
        var claims = new List<Claim>(){
            new(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
            new(ClaimTypes.Name, appUser.UserName)
        };

        var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
        var tokenDiscriptor = new SecurityTokenDescriptor()
        {
            Subject= new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(5),
            SigningCredentials = cred
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDiscriptor);

        return tokenHandler.WriteToken(token);
    }
}