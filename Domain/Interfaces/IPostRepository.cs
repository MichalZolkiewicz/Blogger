using Domain.Entities;

namespace Domain.Interfaces;

public interface IPostRepository
{
    IQueryable<Post> GetAll();
    Task<IEnumerable<Post>> GetAllAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy);
    Task<int> GetAllCountAsync(string filterBy);
    Task<Post> GetByIdAsync(int id);
    Task<Post> AddAsync(Post post);
    Task UpdateAsync(Post post);
    Task DeleteAsync(Post post);
}
