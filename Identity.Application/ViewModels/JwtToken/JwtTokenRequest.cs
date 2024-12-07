namespace Identity.Application.ViewModels.JwtToken;

public class JwtTokenRequest
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}