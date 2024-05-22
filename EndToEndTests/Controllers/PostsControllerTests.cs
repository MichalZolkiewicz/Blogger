using Application.Dto.Posts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using WebAPI.Wrapper;
using Xunit;

namespace EndToEndTests.Controllers;

public class PostsControllerTests
{
    private readonly HttpClient _httpClient;

    public PostsControllerTests()
    {
        var webAppFactory = new WebApplicationFactory<Program>();
        _httpClient = webAppFactory.CreateDefaultClient();
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
