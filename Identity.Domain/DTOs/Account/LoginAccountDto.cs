namespace Identity.Domain.DTOs.Account;

public class LoginAccountDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpirationDate { get; set; }
}