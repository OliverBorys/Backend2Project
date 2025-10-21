using Backend2Project.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend2Project.Data;

public class Backend2ProjectContext(DbContextOptions<Backend2ProjectContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---------- users ----------
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(u => u.Id);
            e.Property(u => u.Username).HasColumnName("username");
            e.Property(u => u.Password).HasColumnName("password");
            e.Property(u => u.Role).HasColumnName("role").HasDefaultValue("customer");
            e.HasIndex(u => u.Username).IsUnique();
        });

        // ---------- categories ----------
        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("categories");
            e.HasKey(c => c.Id);
            e.Property(c => c.CategoryName).HasColumnName("categoryName");
            e.Property(c => c.ImageUrl).HasColumnName("image_url");
            e.HasIndex(c => c.CategoryName).IsUnique();
        });

        // ---------- products ----------
        modelBuilder.Entity<Product>(e =>
        {
            e.ToTable("products");
            e.HasKey(p => p.Id);
            e.Property(p => p.ProductName).HasColumnName("productName");
            e.Property(p => p.Price).HasColumnName("price");
            e.Property(p => p.Image).HasColumnName("image");
            e.Property(p => p.SecondaryImage1).HasColumnName("secondaryImage1");
            e.Property(p => p.SecondaryImage2).HasColumnName("secondaryImage2");
            e.Property(p => p.SecondaryImage3).HasColumnName("secondaryImage3");
            e.Property(p => p.Brand).HasColumnName("brand");
            e.Property(p => p.ProductDescription).HasColumnName("productDescription");
            e.Property(p => p.IsTrending).HasColumnName("isTrending");
            e.Property(p => p.CategoryId).HasColumnName("categoryId");
            e.Property(p => p.PublishingDate).HasColumnName("publishingDate");

            // FK till Category med CASCADE delete
            e.HasOne(p => p.Category)
             .WithMany(c => c.Products)
             .HasForeignKey(p => p.CategoryId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
