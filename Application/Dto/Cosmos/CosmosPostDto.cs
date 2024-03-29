using Application.Mappings;
using AutoMapper;
using Domain.Entities.Cosmos;

namespace Application.Dto.Cosmos;

public class CosmosPostDto : IMap
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CosmosPost, CosmosPostDto>();
    }
}
