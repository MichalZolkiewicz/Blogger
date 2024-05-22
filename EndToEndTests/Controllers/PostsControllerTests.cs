using Application.Dto.Posts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using WebAPI.Wrapper;
using Xunit;

namespace EndToEndTests.Controllers;

[Collection("Sequential")]
public class PostsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _factory;

    public PostsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient();
    }

    [Fact]
    public async Task FetchingPostsShouldNotReturnEmptyCollection()
    {

        var response = await _httpClient.GetAsync(@"api/1.0/Posts");
        var content = await response.Content.ReadAsStringAsync();
        var pagedResponse = JsonConvert.DeserializeObject<PageResponse<IEnumerable<PostDto>>>(content);

        response.EnsureSuccessStatusCode();
        pagedResponse?.Data.Should().NotBeEmpty();
    }
}
