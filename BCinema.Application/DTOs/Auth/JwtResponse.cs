namespace BCinema.Application.DTOs.Auth;

public class JwtResponse
{
    public string Type { get; set; } = default!;
    public string AccessToken { get; set; } = default!;
}