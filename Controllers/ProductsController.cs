using Backend2Project.Data;
using Backend2Project.Data.Entities;
using Backend2Project.Dtos;
using Backend2Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend2Project.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(Backend2ProjectContext db, ProductService productService) : ControllerBase
{
    // GET /api/products or ?page=1&pageSize=5 or ?slug=
    [HttpGet]
    public ActionResult GetAllProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? slug = null)
    {

        if (!string.IsNullOrWhiteSpace(slug))
        {
            var product = productService.GetProductBySlug(slug);
            if (product is null)
                return Ok(new List<ProductDto>()); // Tom lista
                                                   //return NotFound(new { message = "Produkten hittades inte." });

            var map = ProductDto.Projection.Compile();
            return Ok(map(product));
        }

        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var all = productService.GetAllProducts();
        var total = all.Count();

        var mapList = ProductDto.Projection.Compile();
        var items = all
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(mapList)
            .ToList();

        return Ok(new
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = total,
            TotalPages = (int)Math.Ceiling((double)total / pageSize)
        });
    }

    // GET /api/products/{id}
    [HttpGet("{id:int}")]
    public ActionResult GetProductById(int id)
    {
        var product = productService.GetProductById(id);
        if (product is null)
            return NotFound(new { message = "Produkten hittades inte." });

        var map = ProductDto.Projection.Compile();
        return Ok(map(product));
    }

    // POST /api/products
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Product dto, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var (ok, error, created) = await productService.CreateProduct(dto, ct);

        if (!ok)
            return BadRequest(new { message = error });

        return CreatedAtAction(
            nameof(GetProductById),
            new { id = created!.Id },
            created);
    }

    // PATCH /api/products/{id}
    [Authorize(Roles = "admin")]
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] ProductPatchDto dto, CancellationToken ct)
    {
        if (dto is null) return BadRequest(new { message = "Request body is required." });

        var (ok, error, updated) = await productService.UpdateProduct(id, dto, ct);

        if (!ok)
        {
            return error switch
            {
                "Produkten hittades inte." => NotFound(new { message = error }),
                "Ogiltigt categoryId." => BadRequest(new { message = error }),
                "productName får inte vara tomt." => BadRequest(new { message = error }),
                _ => BadRequest(new { message = "Ett okänt fel inträffade." }),
            };
        }

        return Ok(updated);
    }


    // DELETE /api/products/{id}
    [Authorize(Roles = "admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken ct)
    {
        var (ok, error) = await productService.DeleteProduct(id, ct);
        return ok ? NoContent() : NotFound(new { message = error });
    }
}