using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend2Project.Data.Entities;

[Table("categories")]
public class Category
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string CategoryName { get; set; } = default!;

    [MaxLength(250)]
    public string? ImageUrl { get; set; }

    public ICollection<Product> Products { get; set; } = [];
}