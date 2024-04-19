using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AttachmentRepository : IAttachmentRepository
{
    private readonly BloggerContext _context;

    public AttachmentRepository(BloggerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Attachment>> GetByPostIdAsync(int postId)
    {
        return await _context.Attachments.Include(x => x.Posts).Where(x => x.Posts.Select(x => x.Id).Contains(postId)).ToListAsync();
    }

    public async Task<Attachment> GetByIdAsync(int id)
    {
        return await _context.Attachments.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Attachment> AddAsync(Attachment attachment)
    {
        var createdAttachment = await _context.Attachments.AddAsync(attachment);
        await _context.SaveChangesAsync();
        return createdAttachment.Entity;
    }

    public async Task DeleteAsync(Attachment attachment)
    {
        _context.Attachments.Remove(attachment);
        await _context.SaveChangesAsync();
        await Task.CompletedTask;
    }    
}
