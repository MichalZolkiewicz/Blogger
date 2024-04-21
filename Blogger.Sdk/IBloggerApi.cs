using Blogger.Contracts.Requests;
using Blogger.Contracts.Responses;
using Refit;

namespace Blogger.Sdk;

[Headers("Authorization: Bearer")]
public interface IBloggerApi
{
    [Get("/api/1.0/posts/{id}")]
    Task<ApiResponse<Response<PostDto>>> GetPostAsync(int id);

    [Post("/api/1.0/posts")]
    Task<ApiResponse<Response<PostDto>>> CreatePostAsync(CreatePostDto newPost);

    [Put("/api/1.0/posts")]
    Task UpdatePostAsync(UpdatePostDto updatePostDto);

    [Delete("/api/1.0/posts")]
    Task DeletePostAsync(int id);
}
