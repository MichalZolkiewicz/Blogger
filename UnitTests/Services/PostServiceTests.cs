using Application.Dto.Posts;
using Application.Services;
using AutoMapper;
using Castle.Core.Logging;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Services;

public class PostServiceTests
{
    [Fact]
    public async Task AddPostAsyncShouldInvokeAddAsyncOnPostRepository()
    {
        //Arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<PostService>>();

        var postService = new PostService(postRepositoryMock.Object, mapperMock.Object, loggerMock.Object);

        var postDto = new CreatePostDto()
        {
            Title = "Title 1",
            Content = "Content 1"
        };

        mapperMock.Setup(x => x.Map<Post>(postDto)).Returns(new Post() { Title = postDto.Title, Content = postDto.Content });

        //Act
        await postService.AddNewPostAsync(postDto, "86fd94a3-cba8-466e-b089-b6df4d6f340d");

        //Assert
        postRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Post>()), Times.Once);
    }

  
    [Fact]
    public async Task WhenInvokingGetPostAsyncItShouldInvokeGetAsyncOnPostRepository()
    {
        //Arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<PostService>>();

        var postService = new PostService(postRepositoryMock.Object, mapperMock.Object, loggerMock.Object);

        var post = new Post(1, "Title 1", "Content 1");
        var postDto = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
        };

        mapperMock.Setup(x => x.Map<Post>(postDto)).Returns(post);
        postRepositoryMock.Setup(x => x.GetByIdAsync(post.Id)).ReturnsAsync(post);

        //Act
        var existingPostDto = await postService.GetPostByIdAsync(post.Id);

        //Assert
        postRepositoryMock.Verify(x => x.GetByIdAsync(post.Id), Times.Once);
        postDto.Should().NotBeNull();
        postDto.Title.Should().NotBeNull();
        postDto.Title.Should().BeEquivalentTo(post.Title);
        postDto.Content.Should().NotBeNull();
        postDto.Content.Should().BeEquivalentTo(post.Content);
    }
}
