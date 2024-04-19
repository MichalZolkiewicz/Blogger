using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dto.Posts;

public class CreatePostDto : IMap
{
    public string Title { get; set; }
    public string Content { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreatePostDto, Post>();
    }
}
