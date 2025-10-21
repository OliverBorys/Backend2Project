using Microsoft.EntityFrameworkCore;
using Backend2Project.Data;
using Backend2Project.Dtos;
using Backend2Project.Data.Entities;
using Backend2Project.Data.Extensions;
using Backend2Project.Utilities;

namespace Backend2Project.Services;

public class ProductService
{
    private readonly Backend2ProjectContext context;

    public ProductService(Backend2ProjectContext context)
    {
        this.context = context;
    }

    // Hämta alla produkter
    public IEnumerable<Product> GetAllProducts()
    {
        var products = context
            .Products
            .Include(x => x.Category)
            .ToList();

        return products;
    }

    //Hämta produkt via Id
    public Product? GetProductById(int id)
    {
        return context
            .Products
            .Include(x => x.Category)
            .FirstOrDefault(p => p.Id == id);
    }

    // Hämta produkt via slug
    public Product? GetProductBySlug(string slug)
    {
        var normalized = slug.Slugify();

        var matchId = context.Products
            .Select(p => new { p.Id, p.ProductName })
            .AsEnumerable()
            .FirstOrDefault(x => x.ProductName.Slugify() == normalized)
            ?.Id;

        if (matchId is null) return null;

        return context
            .Products
            .Include(x => x.Category)
            .FirstOrDefault(p => p.Id == matchId.Value);
    }


    // Skapa ny produkt
    public async Task<(bool ok, string? error, ProductDto? dto)> CreateProduct(Product entity, CancellationToken ct = default)
    {
        if (entity is null) return (false, "Begäran måste innehålla produktdata.", null);
        if (string.IsNullOrWhiteSpace(entity.ProductName)) return (false, "productName krävs.", null);

        var categoryExists = await context.Categories.AnyAsync(c => c.Id == entity.CategoryId, ct);
        if (!categoryExists) return (false, "Ogiltig categoryId.", null);

        context.Products.Add(entity);
        await context.SaveChangesAsync(ct);

        var created = await context.Products
            .AsNoTracking()
            .Where(p => p.Id == entity.Id)
            .Select(ProductDto.Projection)
            .FirstAsync(ct);

        return (true, null, created);
    }


    // Uppdatera produkt
    public async Task<(bool ok, string? error, ProductDto? dto)> UpdateProduct(int id, ProductPatchDto patch, CancellationToken ct = default)
    {
        var existing = await context.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (existing is null) return (false, "Produkten hittades inte.", null);

        if (patch.CategoryId.HasValue)
        {
            var exists = await context.Categories.AnyAsync(c => c.Id == patch.CategoryId.Value, ct);
            if (!exists) return (false, "Ogiltig categoryId.", null);
        }

        if (patch.ProductName is string name && string.IsNullOrWhiteSpace(name))
            return (false, "productName får inte vara tomt.", null);

        existing.ApplyPatch(patch);
        await context.SaveChangesAsync(ct);

        var updated = await context.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(ProductDto.Projection)
            .FirstAsync(ct);

        return (true, null, updated);
    }

    // Radera produkt
    public async Task<(bool ok, string? error)> DeleteProduct(int id, CancellationToken ct = default)
    {
        var p = await context.Products.FindAsync([id], ct);
        if (p is null) return (false, "Produkten hittades inte.");

        context.Products.Remove(p);
        await context.SaveChangesAsync(ct);
        return (true, null);
    }
}
