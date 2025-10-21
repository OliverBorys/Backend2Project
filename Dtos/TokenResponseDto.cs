namespace Backend2Project.Dtos;

public class TokenResponseDto
{
    public string access_token { get; set; } = default!;
    public string token_type { get; init; } = "Bearer";
    public int expires_in { get; init; }
}
