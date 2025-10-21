using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend2Project.Data.Entities;

[Table("products")]
public class Product
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string ProductName { get; set; } = default!;

    [Required]
    public double Price { get; set; }

    [Required, MaxLength(250)]
    public string Image { get; set; } = default!;

    [MaxLength(250)]
    public string? SecondaryImage1 { get; set; }

    [MaxLength(250)]
    public string? SecondaryImage2 { get; set; }

    [MaxLength(250)]
    public string? SecondaryImage3 { get; set; }

    [Required, MaxLength(50)]
    public string Brand { get; set; } = default!;

    [Required, MaxLength(500)]
    public string ProductDescription { get; set; } = default!;

    public string? IsTrending { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public DateOnly PublishingDate { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }
}