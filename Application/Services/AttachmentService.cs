using Application.Dto.Attachments;
using Application.Dto.Pictures;
using Application.ExtensionMethods;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class AttachmentService : IAttachmentService
{
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public AttachmentService(IAttachmentRepository attachmentRepository, IPostRepository postRepository, IMapper mapper)
    {
        _attachmentRepository = attachmentRepository;
        _postRepository = postRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<AttachmentDto>> GetAttachmentByPostIdAsync(int postId)
    {
        var attachment = await _attachmentRepository.GetByPostIdAsync(postId);
        return _mapper.Map<IEnumerable<AttachmentDto>>(attachment);
    }

    public async Task<DownloadAttachmentDto> DownloadAttchmentByIdAsync(int id)
    {
        var attachment = await _attachmentRepository.GetByIdAsync(id);

        return new DownloadAttachmentDto()
        {
            Name = attachment.Name,
            Content = File.ReadAllBytes(attachment.Path)
        };
    }

    public async Task<AttachmentDto> AddAttachmentToPostAsync(int postId, IFormFile file)
    {
        var post = await _postRepository.GetByIdAsync(postId);

        var attachememt = new Attachment()
        {
            Posts = new List<Post> { post },
            Name = file.FileName,
            Path = file.SaveFile()
        };

        var result = await _attachmentRepository.AddAsync(attachememt);
        return _mapper.Map<AttachmentDto>(result);
    }  

    public async Task DeleteAttachmentAsync(int id)
    {
        var attachment = await _attachmentRepository.GetByIdAsync(id);
        await _attachmentRepository.DeleteAsync(attachment);
        File.Delete(attachment.Path);
    }
}
