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

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IDbUserApi _userApi;

    public UserController(
              UserService userService,
              IDbUserApi userApi
)
    {
        _userService = userService;
        _userApi = userApi;
    }

    [HttpPost]
    [Route("register")]
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
            return Ok(new { Token = token });
        }

        return BadRequest(ModelState);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (ModelState.IsValid)
        {
            var user = await _userApi.GetUserByMail(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid login attempt.");

            var token = _userService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        return BadRequest(ModelState);
    }

   
}
