using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dto.Attachments;

public class AttachmentDto : IMap
{
    public int Id { get; set; }

    public string Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Attachment, AttachmentDto>();
    }
}
