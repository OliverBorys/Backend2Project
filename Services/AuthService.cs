using Backend2Project.Data;
using Backend2Project.Data.Entities;

namespace Backend2Project.Services;

public class AuthService
{
    private readonly Backend2ProjectContext context;

    public AuthService(Backend2ProjectContext context)
    {
        this.context = context;
    }

    public User? Authenticate(string username, string password)
    {
        var User = context.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

        return User;
    }
}
