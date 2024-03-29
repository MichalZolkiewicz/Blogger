using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dto;

public class PostDto : IMap
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreationDate { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Post, PostDto>()
            .ForMember(x => x.CreationDate, y => y.MapFrom(z => z.Created));
    }
}
