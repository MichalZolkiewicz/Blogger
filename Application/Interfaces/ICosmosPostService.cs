using Application.Dto;
using Application.Dto.Cosmos;

namespace Application.Interfaces;

public interface ICosmosPostService
{
    Task<IEnumerable<CosmosPostDto>> GetAllPostsAsync();
    Task<CosmosPostDto> GetPostByIdAsync(string id);
    Task<CosmosPostDto> AddNewPostAsync(CreateCosmosPostDto post);
    Task UpdatePostAsync(UpdateCosmosPostDto updatePost);
    Task DeletePostAsync(string id);
}
