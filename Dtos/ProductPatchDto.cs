using System.ComponentModel.DataAnnotations;

namespace Backend2Project.Dtos;

public class ProductPatchDto
{
    [MaxLength(50)]
    public string? ProductName { get; init; }

    public decimal? Price { get; init; }

    [MaxLength(250)]
    public string? Image { get; init; }
    [MaxLength(250)]
    public string? SecondaryImage1 { get; init; }
    [MaxLength(250)]
    public string? SecondaryImage2 { get; init; }
    [MaxLength(250)]
    public string? SecondaryImage3 { get; init; }

    [MaxLength(50)]
    public string? Brand { get; init; }

    [MaxLength(500)]
    public string? ProductDescription { get; init; }
    
    public string? IsTrending { get; init; }
    
    public int? CategoryId { get; init; }
    
    public DateOnly? PublishingDate { get; init; }
}