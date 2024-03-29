using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Cosmos;

namespace Application.Dto.Cosmos;

public class UpdateCosmosPostDto : IMap
{
    public string Id { get; set; }
    public string Content { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateCosmosPostDto, CosmosPost>();
    }
}
