using Application.Mappings;
using AutoMapper;

namespace Application.Dto.Picture;

public class PictureDto : IMap
{

    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] Image { get; set; }
    public bool Main { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Picture, PictureDto>();
    }
}
