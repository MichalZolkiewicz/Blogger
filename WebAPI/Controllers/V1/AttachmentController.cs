using Application.Dto.Attachments;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebAPI.Wrapper;

namespace WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = UserRoles.User)]
[ApiController]
public class AttachmentController : ControllerBase
{
    private readonly IAttachmentService _attachmentService;
    private readonly IPostService _postService;

    public AttachmentController(IAttachmentService attachmentService, IPostService postService)
    {
        _attachmentService = attachmentService;
        _postService = postService;
    }

    [SwaggerOperation(Summary = "Retrieves the attachments by unique post id")]
    [HttpGet("[action]/{postId}")]
    public async Task<IActionResult> GetByPostIdAsync(int postId)
    {
        var attachments = await _attachmentService.GetAttachmentByPostIdAsync(postId);
        return Ok(new Response<IEnumerable<AttachmentDto>>(attachments));
    }

    [SwaggerOperation(Summary = "Downloads the attachments by unique id")]
    [HttpGet("{postId}/{id}")]
    public async Task<IActionResult> DownloadAsync(int postId, int id)
    {
        var userOwner = await _postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (!userOwner)
        {
            return BadRequest(new Response(false, "You do not own this post."));
        }

        var attachment = await _attachmentService.DownloadAttchmentByIdAsync(id);
        if (attachment == null)
        {
            return NotFound();
        }
        return File(attachment.Content, System.Net.Mime.MediaTypeNames.Application.Octet, attachment.Name);
    }

    [SwaggerOperation(Summary = "Adds a new attachment to post")]
    [HttpPost("{postId}")]
    public async Task<IActionResult> AddToPostAsync(int postId, IFormFile formFile)
    {
        var post = await _postService.GetPostByIdAsync(postId);
        if (post == null)
        {
            return BadRequest(new Response(false, $"Post with id {postId} does not exist."));
        }

        var userOwner = await _postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (!userOwner)
        {
            return BadRequest(new Response(false, "You do not own this post."));
        }

        var attachment = await _attachmentService.AddAttachmentToPostAsync(postId, formFile);
        return Created($"api/attachments/{attachment.Id}", new Response<AttachmentDto>(attachment));
    }

    [SwaggerOperation(Summary = "Deletes attachment from post")]
    [HttpDelete("{postId}/{id}")]
    public async Task<IActionResult> DeleteAsync(int postId, int id)
    {
        var userOwner = await _postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (!userOwner)
        {
            return BadRequest(new Response(false, "You do not own this post."));
        }

        await _attachmentService.DeleteAttachmentAsync(id);
        return NoContent();
    }
}
