using Application.Dto.Pictures;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IPictureService
{
    Task<IEnumerable<PictureDto>> GetPicturesByPostIdAsync(int postId);
    Task<PictureDto> GetPictureByIdAsync(int id);
    Task<PictureDto> AddPictureToPostAsync(int postId, IFormFile file);
    Task SetMainPicutreAsync(int postId, int id);
    Task DeletePictureAsync(int id);
}
