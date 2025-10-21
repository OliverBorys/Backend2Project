using Backend2Project.Data;
using Backend2Project.Dtos;
using Backend2Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend2Project.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(Backend2ProjectContext db, CategoryService categoryService) : ControllerBase
{
    public record CreateCategoryRequest(string CategoryName, string? ImageUrl);
    public record PatchCategoryRequest(string CategoryName, string? ImageUrl);


    // GET /api/categories[?slug=]
    [HttpGet]
    public async Task<ActionResult> GetAllCategories([FromQuery] string? slug = null)
    {
        if (!string.IsNullOrWhiteSpace(slug))
        {
            var entities = categoryService.GetCategoryBySlug(slug);
            var map = CategoryDto.Projection.Compile();
            var list = entities.Select(map).ToList();
            return Ok(list);
        }

        var allEntities = categoryService.GetAllCategories();
        var mapAll = CategoryDto.Projection.Compile();
        var all = allEntities.Select(mapAll).ToList();
        return Ok(all);
    }

    // GET /api/categories/{id}
    [HttpGet("{id:int}")]
    public ActionResult GetCategoryById(int id)
    {
        var entity = categoryService.GetCategoryById(id);
        if (entity is null) return NotFound();

        var map = CategoryDto.Projection.Compile();
        return Ok(map(entity));
    }

    // POST /api/categories
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryRequest dto, CancellationToken ct)
    {
        var (ok, error, entity) = await categoryService.CreateCategory(dto.CategoryName, dto.ImageUrl, ct);

        if (!ok)
        {
            return error == "categoryName måste vara unik."
                ? Conflict(new { message = error })
                : BadRequest(new { message = error });
        }

        var map = CategoryDto.Projection.Compile();
        var createdDto = map(entity!);

        return CreatedAtAction(
            nameof(GetCategoryById),
            new { id = entity!.Id },
            createdDto);
    }

    // PATCH /api/categories/{id}
    [Authorize(Roles = "admin")]
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] PatchCategoryRequest dto, CancellationToken ct)
    {
        var (ok, error, entity) = await categoryService.UpdateCategory(id, dto.CategoryName, dto.ImageUrl, ct);

        if (!ok)
        {
            return error switch
            {
                "Kategorin hittades inte." => NotFound(new { message = error }),
                "categoryName måste vara unik." => Conflict(new { message = error }),
                _ => BadRequest(new { message = error })
            };
        }

        var map = CategoryDto.Projection.Compile();
        return Ok(map(entity!));
    }


    // DELETE /api/categories/{id}
    [Authorize(Roles = "admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id, CancellationToken ct)
    {
        var (ok, error) = await categoryService.DeleteCategory(id, ct);
        return ok ? NoContent() : NotFound(new { message = error });
    }
}