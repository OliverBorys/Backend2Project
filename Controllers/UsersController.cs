using Backend2Project.Data;
using Backend2Project.Dtos;
using Backend2Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend2Project.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(Backend2ProjectContext db, UserService userService) : ControllerBase
{
    public record CreateUserRequest(string Username, string Password);
    public record PatchUserRequest(string? Username, string? Password);

    // GET /api/users[?page=1&pageSize=20]&?slug=
    [HttpGet]
    public ActionResult GetAllUsers([FromQuery] string? slug = null)
    {
        if (!string.IsNullOrWhiteSpace(slug))
        {
            var user = userService.GetUserBySlug(slug);
            return user is null
                ? NotFound(new { message = "Användaren hittades inte." })
                : Ok(new UserDto { Id = user.Id, Username = user.Username });
        }

        var users = userService.GetAllUsers()
            .Select(u => new UserDto { Id = u.Id, Username = u.Username })
            .ToList();

        return Ok(users);
    }

    // GET /api/users/{id}
    [HttpGet("{id:int}")]
    public ActionResult GetUserById(int id)
    {
        var user = userService.GetUserById(id);
        return user is null
            ? NotFound(new { message = "Användaren hittades inte." })
            : Ok(new UserDto { Id = user.Id, Username = user.Username });
    }

    // POST /api/users
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateUserRequest req, CancellationToken ct)
    {
        var (ok, error, user) = await userService.CreateUser(req.Username, req.Password, ct);

        if (!ok)
        {
            return error == "Användarnamnet är redan taget."
                ? Conflict(new { message = error })
                : BadRequest(new { message = error });
        }

        return CreatedAtAction(nameof(GetUserById), new { id = user!.Id },
            new UserDto { Id = user.Id, Username = user.Username });
    }

    // PATCH /api/users/{id}
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] PatchUserRequest req, CancellationToken ct)
    {
        var (ok, error, user) = await userService.UpdateUser(id, req.Username, req.Password, ct);

        if (!ok)
        {
            return error switch
            {
                "Användaren hittades inte." => NotFound(new { message = error }),
                "Användarnamnet är redan taget." => Conflict(new { message = error }),
                _ => BadRequest(new { message = error })
            };
        }

        return Ok(new UserDto { Id = user!.Id, Username = user.Username });
    }


    // DELETE /api/users/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken ct)
    {
        var (ok, error) = await userService.DeleteUser(id, ct);
        return ok ? NoContent() : NotFound(new { message = error });
    }
}