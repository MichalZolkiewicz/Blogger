using Application.Dto;

namespace Application.Interfaces;

public interface IPostService
{
    IEnumerable<PostDto> GetAllPosts();
    PostDto GetPostById(int id);
    PostDto AddNewPost(CreatePostDto post);
    void UpdatePost(UpdatePostDto updatePost);
    void DeletePost(int id);
    IEnumerable<PostDto> SearchInTitle(string title);
}
