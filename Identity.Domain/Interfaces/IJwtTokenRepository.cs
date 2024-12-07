namespace Identity.Domain.Interfaces;

public interface IJwtTokenRepository : IRepositoryBase<long, JwtToken>
{
    Task DestroyAsync(string token);
    Task<bool> CheckTokenValidation(string token);
    Task<bool> CheckDuplicateToken(string email, string device);
}