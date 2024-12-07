namespace Identity.Infrastructure.Repositories;

public class JwtTokenRepository(IdentityContext _context)
    : RepositoryBase<long, JwtToken>(_context), IJwtTokenRepository
{
    public async Task DestroyAsync(string token)
    {
        var account = await _context.JwtTokens.FirstOrDefaultAsync(x => x.AccessToken == token);
        _context.JwtTokens.Remove(account);
    }

    public async Task<bool> CheckTokenValidation(string token)
    {
        var myToken = await _context.JwtTokens.OrderBy(x => x.CreationDate)
            .LastOrDefaultAsync(x => x.AccessToken.StartsWith(token));

        return myToken != null && myToken.AccessTokenExpirationDate >= DateTime.Now;
    }

    public async Task<bool> CheckDuplicateToken(string email, string device)
    {
        return await _context.JwtTokens.Include(x => x.Account)
            .AnyAsync(x => x.Account.Email == email && x.Device == device);
    }
}