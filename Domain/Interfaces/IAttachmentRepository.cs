using Domain.Entities;

namespace Domain.Interfaces;

public interface IAttachmentRepository
{
    Task<IEnumerable<Attachment>> GetByPostIdAsync(int postId);
    Task<Attachment> GetByIdAsync(int id);
    Task<Attachment> AddAsync(Attachment attachment);
    Task DeleteAsync(Attachment attachment);
}
