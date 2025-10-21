using Backend2Project.Data;
using Backend2Project.Data.Entities;
using Backend2Project.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Backend2Project.Services;

public class UserService
{
    private readonly Backend2ProjectContext context;

    public UserService(Backend2ProjectContext context)
    {
        this.context = context;
    }

    // Hämta alla användare
    public IEnumerable<User> GetAllUsers()
    {
        return context.Users.ToList();
    }

    // Hämta användare via Id
    public User? GetUserById(int id)
    {
        return context.Users.FirstOrDefault(u => u.Id == id);
    }

    // Hämta användare via slug
    public User? GetUserBySlug(string slug)
    {
        var normalized = slug.Slugify();

        var matchId = context.Users
            .Select(u => new { u.Id, u.Username })
            .AsEnumerable()
            .FirstOrDefault(x => x.Username.Slugify() == normalized)
            ?.Id;

        if (matchId is null) return null;
        return context.Users.FirstOrDefault(u => u.Id == matchId.Value);
    }

    // Skapa ny användare
    public async Task<(bool ok, string? error, User? user)> CreateUser(string username, string password, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return (false, "username och password krävs.", null);

        var trimmed = username.Trim();

        var taken = await context.Users.AnyAsync(u => u.Username == trimmed, ct);
        if (taken) return (false, "Användarnamnet är redan taget.", null);

        var user = new User { Username = trimmed, Password = password, Role = "customer" };
        context.Users.Add(user);
        await context.SaveChangesAsync(ct);

        return (true, null, user);
    }

    // Updatera användare
    public async Task<(bool ok, string? error, User? user)> UpdateUser(int id, string? newUsername, string? newPassword, CancellationToken ct = default)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is null) return (false, "Användaren hittades inte.", null);

        var changed = false;

        if (!string.IsNullOrWhiteSpace(newUsername))
        {
            var trimmed = newUsername.Trim();
            if (!string.Equals(user.Username, trimmed, StringComparison.Ordinal))
            {
                var taken = await context.Users.AnyAsync(u => u.Username == trimmed && u.Id != id, ct);
                if (taken) return (false, "Användarnamnet är redan taget.", null);

                user.Username = trimmed;
                changed = true;
            }
        }

        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            user.Password = newPassword;
            changed = true;
        }

        if (!changed) return (false, "Inga giltiga fält att uppdatera.", null);

        await context.SaveChangesAsync(ct);
        return (true, null, user);
    }

    // Radera användare
    public async Task<(bool ok, string? error)> DeleteUser(int id, CancellationToken ct = default)
    {
        var user = await context.Users.FindAsync([id], ct);
        if (user is null)
            return (false, "Användaren hittades inte.");

        context.Users.Remove(user);
        await context.SaveChangesAsync(ct);

        return (true, null);
    }
}