using Domain.Common;

namespace Domain.Entities;

public class Post : AuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public Post() { }

    public Post(int id, string title, string contect)
    {
        Id = id;
        Title = title;
        Content = contect;
    }
}
