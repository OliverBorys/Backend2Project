using Backend2Project.Data.Entities;
using Backend2Project.Dtos;

namespace Backend2Project.Data.Extensions;

public static class ProductPatchingExtensions
{
    public static void ApplyPatch(this Product entity, ProductPatchDto dto)
    {
        if (dto.ProductName is not null) entity.ProductName = dto.ProductName;
        if (dto.Price.HasValue) entity.Price = dto.Price.Value;
        if (dto.Image is not null) entity.Image = dto.Image;
        if (dto.SecondaryImage1 is not null) entity.SecondaryImage1 = dto.SecondaryImage1;
        if (dto.SecondaryImage2 is not null) entity.SecondaryImage2 = dto.SecondaryImage2;
        if (dto.SecondaryImage3 is not null) entity.SecondaryImage3 = dto.SecondaryImage3;
        if (dto.Brand is not null) entity.Brand = dto.Brand;
        if (dto.ProductDescription is not null) entity.ProductDescription = dto.ProductDescription;
        if (dto.IsTrending is not null) entity.IsTrending = dto.IsTrending;
        if (dto.PublishingDate.HasValue) entity.PublishingDate = dto.PublishingDate.Value;

        if (dto.CategoryId.HasValue) entity.CategoryId = dto.CategoryId.Value;
    }
}
