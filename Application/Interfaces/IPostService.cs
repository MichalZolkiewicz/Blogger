﻿using Application.Dto.Posts;
using Domain.Entities;

namespace Application.Interfaces;

public interface IPostService
{
    IQueryable<PostDto> GetAllPosts();
    Task<IEnumerable<PostDto>> GetAllPostsAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy);
    Task<int> GetAllPostsCountAsync(string filterBy);
    Task<PostDto> GetPostByIdAsync(int id);
    Task<PostDto> AddNewPostAsync(CreatePostDto post, string userId);
    Task UpdatePostAsync(UpdatePostDto updatePost);
    Task DeletePostAsync(int id);
    Task<bool> UserOwnsPostAsync(int postId, string userId);
}
