using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Posts")]
public class Post : AuditableEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Content { get; set; }

    [Required]
    [MaxLength(450)]
    public string UserId { get; set; }

    public Post() { }

    public Post(int id, string title, string contect)
    {
        Id = id;
        Title = title;
        Content = contect;
    }
}
