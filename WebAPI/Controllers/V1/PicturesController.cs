using Application.Dto.Pictures;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebAPI.Wrapper;

namespace WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.User)]
[ApiController]
public class PicturesController : ControllerBase
{
    private readonly IPictureService _pictureService;
    private readonly IPostService _postService;

    public PicturesController(IPictureService pictureService, IPostService postService)
    {
        _pictureService = pictureService;
        _postService = postService;
    }

    [SwaggerOperation(Summary = "Retrieves a pictures by unique post id")]
    [HttpGet("[action]/{postId}")]
    public async Task<IActionResult> GetByPostIdAsync(int postId)
    {
        var pictures = await _pictureService.GetPicturesByPostIdAsync(postId);
        return Ok(new Response<IEnumerable<PictureDto>>(pictures));
    }

    [SwaggerOperation(Summary = "Retrieves a specific picture by unique id")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var picture = await _pictureService.GetPictureByIdAsync(id);
        if (picture == null)
        {
            return NotFound();
        }
        return Ok(new Response<PictureDto>(picture));
    }

    [SwaggerOperation(Summary = "Add new picture to post")]
    [HttpPost("{postId}")]
    public async Task<IActionResult> AddToPostAsync(int postId, IFormFile formFile)
    {
        var post = await _postService.GetPostByIdAsync(postId);
        if (post == null)
        {
            return BadRequest(new Response(false, $"Post with id {postId} does not exist."));
        }

        var userOwner = await _postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));

        if(!userOwner)
        {
            return BadRequest(new Response(false, "You do not own this post."));
        }

        var picture = await _pictureService.AddPictureToPostAsync(postId, formFile);
        return Created($"api/picture/{picture.Id}", new Response<PictureDto>(picture));
    }

    [SwaggerOperation(Summary = "Sets the main picture of the post")]
    [HttpPut("[action]/{postId}/{id}")]

    public async Task<IActionResult> SetMainPicture(int postId, int id)
    {
        var userOwner = await _postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (!userOwner)
        {
            return BadRequest(new Response(false, "You do not own this post."));
        }

        await _pictureService.SetMainPicutreAsync(postId, id);
        return NoContent();
    }

    [SwaggerOperation(Summary = "Delete specific picture")]
    [HttpDelete("{postId}/{id}")]
    public async Task<IActionResult> Delete(int id, int postId)
    {
        var userOwnsPost = await _postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));
        if(!userOwnsPost)
        {
            return BadRequest(new Response(false, "You do not own this post."));
        }

        await _pictureService.DeletePictureAsync(id);
        return NoContent();
    }
}
