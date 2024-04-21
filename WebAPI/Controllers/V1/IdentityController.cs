using Application.Interfaces;
using Domain.Enums;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Models;
using WebAPI.SwaggerExamples.Responses;
using WebAPI.Wrapper;

namespace WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailSenderService _emailSender;

    public IdentityController(UserManager<ApplicationUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, IEmailSenderService emailSender)
    {
        _userManager = userManager;
        _configuration = configuration;
        _roleManager = roleManager;
        _emailSender = emailSender;
    }

    /// <summary>
    /// Register the user in the system
    /// </summary>
    /// <response code="200">User created successfully!</response>
    /// <response code="500">User already exists!</response>
    [ProducesResponseType(typeof(RegisterResponseStatus200), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RegisterResponseStatus500), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> RegisterAsync(RegisterModel registerModel)
    {
        var userExists = await _userManager.FindByNameAsync(registerModel.UserName);
        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response
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

        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

        await _userManager.AddToRoleAsync(user, UserRoles.User);

        await _emailSender.Send(user.Email, "Registration confirmation", EmailTemplate.WelcomeMessage, user);

        return Ok(new Response { Succeeded = true, Message = "User created successfuly!" });
    }

    /// <summary>
    /// Register the admin in the system
    /// </summary>
    [HttpPost]
    [Route("RegisterAdmin")]
    public async Task<IActionResult> RegisterAdminAsync(RegisterModel registerModel)
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
        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>
            {
                Succeeded = false,
                Message = "User creation failed! Please check login details and try again",
                Errors = result.Errors.Select(x => x.Description)
            });
        }

        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

        await _userManager.AddToRoleAsync(user, UserRoles.Admin);

        return Ok(new Response<bool> { Succeeded = true, Message = "User created successfuly!" });
    }

    /// <summary>
    /// Register the superuser in the system
    /// </summary>
    [HttpPost]
    [Route("RegisterSuperUser")]
    public async Task<IActionResult> RegisterSuperUserAsync(RegisterModel registerModel)
    {
        var userExists = await _userManager.FindByNameAsync(registerModel.UserName);
        if(userExists != null)
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
        if (result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>
            {
                Succeeded = false,
                Message = "User creation failed! Please check login details and try again",
                Errors = result.Errors.Select(x => x.Description)
            });
        }

        if (!await _roleManager.RoleExistsAsync(UserRoles.AdminOrUser))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.AdminOrUser));

        await _userManager.AddToRoleAsync(user, UserRoles.AdminOrUser);

        return Ok(new Response<bool> { Succeeded = true, Message = "User created successfuly!" });
    }

    /// <summary>
    /// Logs the user into system
    /// </summary>
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> LoginAsync(LoginModel login)
    {
        var user = await _userManager.FindByNameAsync(login.UserName);
        if(user != null && await _userManager.CheckPasswordAsync(user, login.Password)) 
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach(var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return Ok(new AuthSuccessResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }
}
   
