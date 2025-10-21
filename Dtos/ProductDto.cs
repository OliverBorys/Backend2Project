using System.Linq.Expressions;

namespace Backend2Project.Dtos;

public sealed class ProductDto
{
    public int Id { get; init; }
    public string? ProductName { get; init; }
    public double? Price { get; init; }
    public string? Image { get; init; }
    public string? SecondaryImage1 { get; init; }
    public string? SecondaryImage2 { get; init; }
    public string? SecondaryImage3 { get; init; }
    public string? Brand { get; init; }
    public string? ProductDescription { get; init; }
    public string? IsTrending { get; init; }
    public int? CategoryId { get; init; }
    public DateOnly? PublishingDate { get; init; }

    public static Expression<Func<Product, ProductDto>> Projection => p => new ProductDto
    {
        Id = p.Id,
        ProductName = p.ProductName,
        Price = p.Price,
        Image = p.Image,
        SecondaryImage1 = p.SecondaryImage1,
        SecondaryImage2 = p.SecondaryImage2,
        SecondaryImage3 = p.SecondaryImage3,
        Brand = p.Brand,
        ProductDescription = p.ProductDescription,
        IsTrending = p.IsTrending,
        CategoryId = p.CategoryId,
        PublishingDate = p.PublishingDate
    };
}
