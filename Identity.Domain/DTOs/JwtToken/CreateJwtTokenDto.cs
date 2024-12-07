namespace Identity.Domain.DTOs.JwtToken;

public class CreateJwtTokenDto
{
    public string JwtToken { get; set; }
    public DateTime JwtTokenExpirationDate { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpirationDate { get; set; }
    public long AccountId { get; set; }
}