namespace Identity.Domain.DTOs.JwtToken;

public class JwtTokenInfoDto
{
    public string JwtToken { get; set; }
    public string JwtTokenExpirationDate { get; set; }
    public string JwtTokenCreationDate { get; set; }
    public string RefreshToken { get; set; }
    public string RefreshTokenExpirationDate { get; set; }
    public string RefreshTokenCreationDate { get; set; }
    public string Device { get; set; }
    public long AccountId { get; set; }
    public AccountInfoDto Account { get; set; }
}