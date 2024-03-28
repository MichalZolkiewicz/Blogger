using Application.Dto;

namespace Application.Interfaces;

public interface IPostService
{
    Task<IEnumerable<PostDto>> GetAllPostsAsync();
    Task<PostDto> GetPostByIdAsync(int id);
    Task<PostDto> AddNewPostAsync(CreatePostDto post);
    Task UpdatePostAsync(UpdatePostDto updatePost);
    Task DeletePostAsync(int id);
}
