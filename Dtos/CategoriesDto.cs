using Backend2Project.Data.Entities;
using System.Linq.Expressions;

namespace Backend2Project.Dtos;

public sealed class ProductInCategoryDto
{
    public int Id { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string? Image { get; init; }
    public static Expression<Func<Product, ProductInCategoryDto>> Projection => p => new ProductInCategoryDto
    {
        Id = p.Id,
        ProductName = p.ProductName,
        Price = p.Price,
        Image = p.Image
    };
}

public sealed class CategoryDto
{
    public int Id { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string? ImageUrl { get; init; }
    public IReadOnlyList<ProductInCategoryDto> Products { get; init; } = [];

    public static Expression<Func<Category, CategoryDto>> Projection => c => new CategoryDto
    {
        Id = c.Id,
        CategoryName = c.CategoryName,
        ImageUrl = c.ImageUrl,
        Products = c.Products
            .AsQueryable()
            .OrderBy(p => p.Id)
            .Select(ProductInCategoryDto.Projection)
            .ToList()
    };
}