using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Models;
using WebAPI.Wrapper;

namespace WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public IdentityController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register(RegisterModel registerModel)
    {
        var userExists = await _userManager.FindByNameAsync(registerModel.UserName);
        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>
            {
                Succeeded = false,
                Message = "User already exists!"
            });
        }

        var user = new ApplicationUser()
        {
            Email = registerModel.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = registerModel.UserName
        };

        var result = await _userManager.CreateAsync(user, registerModel.Password);
        if(!result.Succeeded) 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>
            {
                Succeeded = false,
                Message = "User creation failed! Please check login details and try again",
                Errors = result.Errors.Select(x => x.Description)
            });
        }

        return Ok(new Response<bool> { Succeeded = true, Message = "User created successfuly!" });
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginModel login)
    {
        var user = await _userManager.FindByNameAsync(login.UserName);
        if(user != null && await _userManager.CheckPasswordAsync(user, login.Password)) 
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }
}
   
