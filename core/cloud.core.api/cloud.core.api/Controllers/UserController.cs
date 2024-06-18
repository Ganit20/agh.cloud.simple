using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using cloud.core.objects.Model;
using cloud.core.api.Services;
using Microsoft.EntityFrameworkCore;
using cloud.core.database.interf;
using RestEase;
using RestEase.Implementation;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IDbUserApi _userApi= RestClient.For<IDbUserApi>("http://cloud.core.database");


    public UserController(
              UserService userService
)
    {
        _userService = userService;
    }
    

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (ModelState.IsValid)
        {
            var userExists = await _userApi.GetUserByMail(request.Email);
            if (userExists is not null)
                return BadRequest("User already exists.");

            var user = new AddUserRequest
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            var u =await _userApi.Adduser(user);

            var token = _userService.GenerateJwtToken(u);
            var newRefreshToken = _userService.GenerateRefreshToken();
            await _userApi.PutSaveRefreshToken(newRefreshToken,u.Id);
            return Ok(new TokenViewModel()
            {
                AccessToken = token,
                RefreshToken = newRefreshToken
            });

        }

        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (ModelState.IsValid)
        {
            var user = await _userApi.GetUserByMail(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid login attempt.");

            var token = _userService.GenerateJwtToken(user);
            var newRefreshToken = _userService.GenerateRefreshToken();
            await _userApi.PutSaveRefreshToken(newRefreshToken, user.Id);
            return Ok(new TokenViewModel()
            {
                AccessToken = token,
                RefreshToken = newRefreshToken
            });
        }

        return BadRequest(ModelState);
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] TokenViewModel request)
    {
        var principal = _userService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null)
            return BadRequest("Invalid access token");

        var expiryDateUnix = long.Parse(principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);

        if (expiryDateTimeUtc > DateTime.UtcNow)
            return BadRequest("This token hasn't expired yet");

        var newAccessToken = _userService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _userService.GenerateRefreshToken();
        await _userApi.PutSaveRefreshToken(newRefreshToken, int.Parse(principal.Claims.FirstOrDefault(x=>x.Type=="id").Value));

        return Ok(new TokenViewModel()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }
    [Authorize]
    [HttpGet("file/info")]
    public async Task<IActionResult> GetUserFileInfo()
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

        return Ok(await _userApi.GetUserFileInfo(userId));
            
     }

}
