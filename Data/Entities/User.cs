using System.ComponentModel.DataAnnotations;

namespace Backend2Project.Data.Entities;

public class User
{
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string Username { get; set; }
    [Required, MaxLength(50)]
    public string Password { get; set; } // plaintext per schema
    [Required]
    public string Role { get; set; } = "customer"; // 'customer' | 'admin'
}
