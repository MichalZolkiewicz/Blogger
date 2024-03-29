using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;
using WebAPI.Filters.Helpers;
using WebAPI.Wrapper;

namespace WebAPI.Controllers.V1;

[Route("api/{v:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [SwaggerOperation(Summary = "Retireves sort fields")]
    [HttpGet("[action]")]
    public IActionResult GetSortFields()
    {
        return Ok(SortingHelper.GetSortFields().Select(x => x.Key));
    }

    [SwaggerOperation(Summary = "Retrieves paged posts")]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SortingFilter sortingFilter, [FromQuery] string filterBy = "")
    {
        var validPaginationFiler = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
        var validSortingFilter = new SortingFilter(sortingFilter.SortField, sortingFilter.Ascending);

        var posts = await _postService.GetAllPostsAsync(validPaginationFiler.PageNumber, validPaginationFiler.PageSize,
                                                        validSortingFilter.SortField, validSortingFilter.Ascending,
                                                        filterBy);
        var totalRecords = await _postService.GetAllPostsCountAsync(filterBy);

        return Ok(PaginationHelper.CreatePageResponse(posts, validPaginationFiler, totalRecords));
    }

    [SwaggerOperation(Summary = "Retrieves all posts")]
    [EnableQuery]
    [HttpGet("[action]")]
    public IQueryable<PostDto> GetAll()
    {
        return _postService.GetAllPosts();
    }

    [SwaggerOperation(Summary = "Retrieves a specific post by unique id")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        return Ok(new Response<PostDto>(post));
    }

    [SwaggerOperation(Summary = "Create a new post")]
    [HttpPost]
    public async Task<IActionResult> Create(CreatePostDto newPost)
    {
        var post = await _postService.AddNewPostAsync(newPost);
        return Created($"api/posts/{post.Id}", new Response<PostDto>(post));
    }

    [SwaggerOperation(Summary = "Update an existing post")]
    [HttpPut]
    public async Task<IActionResult> Update(UpdatePostDto updatePost)
    {
        await _postService.UpdatePostAsync(updatePost);
        return NoContent();
    }

    [SwaggerOperation(Summary = "Delete an existing post by a unique id")]
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        await _postService.DeletePostAsync(id);
        return NoContent();
    }
}
