using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dto.Pictures;

public class PictureDto : IMap
{

    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] Image { get; set; }
    public bool Main { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Picture, PictureDto>();
    }
}
