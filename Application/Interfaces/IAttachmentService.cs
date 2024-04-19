using Application.Dto.Attachments;
using Application.Dto.Pictures;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IAttachmentService
{
    Task<IEnumerable<AttachmentDto>> GetAttachmentByPostIdAsync(int postId);
    Task<DownloadAttachmentDto> DownloadAttchmentByIdAsync(int id);
    Task<AttachmentDto> AddAttachmentToPostAsync(int postId, IFormFile file);
    Task DeleteAttachmentAsync(int id);
}
