using Domain.Entities;

namespace Domain.Interfaces;

public interface IPictureRepository
{
    Task<IEnumerable<Picture>> GetByPostIdAsync(int postId);
    Task<Picture> GetByIdAsync(int id);
    Task<Picture> AddAsync(Picture picture);
    Task SetMainPictureAsync(int postId, int id);
    Task DeleteAsync(Picture picture);
}
