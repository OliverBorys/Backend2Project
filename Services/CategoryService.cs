using Microsoft.EntityFrameworkCore;
using Backend2Project.Data;
using Backend2Project.Data.Entities;
using Backend2Project.Utilities;

namespace Backend2Project.Services;

public class CategoryService
{
    private readonly Backend2ProjectContext context;
    public CategoryService(Backend2ProjectContext context)
    {
        this.context = context;
    }

    // Hämta alla kategorier med deras produkter
    public IEnumerable<Category> GetAllCategories()
    {
        return context
            .Categories
            .Include(c => c.Products)
            .ToList();
    }

    // Hämta kategori via ID med dess produkter
    public Category? GetCategoryById(int id)
    {
        return context
            .Categories
            .Include(c => c.Products)
            .FirstOrDefault(c => c.Id == id);
    }

    // Hämta kategori via slug
    public IEnumerable<Category> GetCategoryBySlug(string slug)
    {
        var normalized = slug.Slugify();

        return context.Categories
            .Where(c => c.CategoryName.ToLower() == normalized)
            .Include(c => c.Products)
            .ToList();
    }


    // Skapa ny kategori
    public async Task<(bool ok, string? error, Category? category)> CreateCategory(string categoryName, string? imageUrl, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
            return (false, "categoryName krävs.", null);

        var trimmed = categoryName.Trim();
        var taken = await context.Categories.AnyAsync(c => c.CategoryName == trimmed, ct);
        if (taken) return (false, "categoryName måste vara unik.", null);

        var entity = new Category { CategoryName = trimmed, ImageUrl = imageUrl };
        context.Categories.Add(entity);
        await context.SaveChangesAsync(ct);

        return (true, null, entity);
    }


    // Uppdatera kategori
    public async Task<(bool ok, string? error, Category? category)> UpdateCategory(int id, string? newName, string? newImageUrl, CancellationToken ct = default)
    {
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (category is null) return (false, "Kategorin hittades inte.", null);

        var changed = false;

        if (newName is not null)
        {
            var trimmed = newName.Trim();
            if (string.IsNullOrEmpty(trimmed))
                return (false, "categoryName får inte vara tom.", null);

            if (!string.Equals(category.CategoryName, trimmed, StringComparison.OrdinalIgnoreCase))
            {
                var taken = await context.Categories.AnyAsync(c => c.CategoryName == trimmed && c.Id != id, ct);
                if (taken) return (false, "categoryName måste vara unik.", null);

                category.CategoryName = trimmed;
                changed = true;
            }
        }

        if (newImageUrl is not null && !string.Equals(category.ImageUrl, newImageUrl, StringComparison.Ordinal))
        {
            category.ImageUrl = newImageUrl;
            changed = true;
        }

        if (!changed) return (false, "Inga giltiga fält att uppdatera.", null);

        await context.SaveChangesAsync(ct);
        return (true, null, category);
    }


    // Radera kategori
    public async Task<(bool ok, string? error)> DeleteCategory(int id, CancellationToken ct = default)
    {
        var category = await context.Categories.FindAsync([id], ct);
        if (category is null) return (false, "Kategorin hittades inte.");

        context.Categories.Remove(category);
        await context.SaveChangesAsync(ct);
        return (true, null);
    }
}
