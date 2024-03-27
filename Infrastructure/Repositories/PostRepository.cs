using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories;

public class PostRepository : IPostRepository
{

    private static readonly ISet<Post> _posts = new HashSet<Post>()
    {
        new Post(1, "Jak zostać programistą", "Treść 1"),
        new Post(2, "Ile zarabia programist", "Treść 2"),
        new Post(3, "Dlateczo warto zostać programistą", "Treść 3"),

    };

    public IEnumerable<Post> GetAll()
    {
        return _posts;
    }

    public Post GetById(int id)
    {
        return _posts.SingleOrDefault(x => x.Id == id);
    }

    public Post Add(Post post)
    {
        post.Id = _posts.Count() + 1;
        post.Created = DateTime.UtcNow;
        _posts.Add(post);
        return post;
    }

    public void Update(Post post)
    {
        post.LastModified = DateTime.UtcNow;
    }

    public void Delete(Post post)
    {
        _posts.Remove(post);
    }
}
