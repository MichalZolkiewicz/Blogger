using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Pictures")]
public class Picture
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public byte[] Image { get; set; }

    [Required]
    public bool Main { get; set; }

    public ICollection<Post> Posts { get; set; }
}
