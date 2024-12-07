namespace Identity.Domain.Entities;

public sealed class JwtToken : BaseEntity
{
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpirationDate { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpirationDate { get; set; }
    public string Device { get; set; }
    public long AccountId { get; set; }
    public Account Account { get; set; }
}