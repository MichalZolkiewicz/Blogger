using Application.Mappings;
using AutoMapper;

namespace Application.Dto.Post;

public class UpdatePostDto : IMap
{
    public int Id { get; set; }
    public string Content { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdatePostDto, Domain.Entities.Post>();
    }
}
