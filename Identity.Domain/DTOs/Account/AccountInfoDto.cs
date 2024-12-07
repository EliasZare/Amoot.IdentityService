namespace Identity.Domain.DTOs.Account;

public class AccountInfoDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string Password { get; set; }
    public bool IsDeleted { get; set; }
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
    public string RefreshTokenExpirationDate { get; set; }
    public string JwtTokenExpirationDate { get; set; }
    public List<JwtTokenInfoDto> Tokens { get; set; }
}