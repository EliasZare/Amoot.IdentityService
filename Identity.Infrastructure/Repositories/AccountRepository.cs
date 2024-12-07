namespace Identity.Infrastructure.Repositories;

public class AccountRepository(IdentityContext _context)
    : RepositoryBase<long, Account>(_context), IAccountRepository
{
    public async Task<AccountInfoDto> GetByEmail(string email)
    {
        return await _context.Accounts.Include(x => x.Tokens).Select(x => new AccountInfoDto
        {
            Email = x.Email,
            Id = x.Id,
            Name = x.Name
        }).FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<Account> GetAccountByEmailAsync(string email)
    {
        return await _context.Accounts.FirstOrDefaultAsync(x => x.Email == email);
    }
    public async Task<List<AccountInfoDto>> GetAccountsAsync()
    {
        const string deviceName = "Win32NT";

        return await _context.Accounts.Include(x => x.Tokens).Select(x => new AccountInfoDto
        {
            Email = x.Email,
            Id = x.Id,
            IsDeleted = x.IsDeleted,
            Tokens = MapJwtTokenInfoDtos(x.Tokens.ToList()),
            JwtToken = GetJwtTokenInfo(x.Tokens, x.Id, deviceName)
                .JwtToken,
            RefreshToken = GetJwtTokenInfo(x.Tokens, x.Id, deviceName)
                .RefreshToken,
            Name = x.Name,
            RefreshTokenExpirationDate =
                GetJwtTokenInfo(x.Tokens, x.Id, deviceName).RefreshTokenExpirationDate,
            JwtTokenExpirationDate = GetJwtTokenInfo(x.Tokens, x.Id, deviceName).JwtTokenExpirationDate
        }).AsSplitQuery().ToListAsync();
    }

    public async Task<JwtTokenInfoDto> GetTokensBy(long accountId)
    {
        var deviceName = "Win32NT";
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == accountId);
        return GetJwtTokenInfo(account.Tokens, accountId, deviceName);
    }

    public async Task<AccountInfoDto> GetAccountByToken(string token)
    {
        var result = await _context.Accounts.Include(x => x.Tokens).Select(x => new AccountInfoDto
        {
            Email = x.Email,
            Id = x.Id,
            IsDeleted = x.IsDeleted,
            Tokens = MapJwtTokenInfoDtos(x.Tokens),
            JwtToken = GetJwtTokenInfo(x.Tokens, x.Id, "Win32NT")
                .JwtToken,
            RefreshToken = GetJwtTokenInfo(x.Tokens, x.Id, "Win32NT")
                .RefreshToken,
            Mobile = x.Mobile,
            Name = x.Name,
            Password = x.Password,
            RefreshTokenExpirationDate =
                GetJwtTokenInfo(x.Tokens, x.Id, "Win32NT").RefreshTokenExpirationDate,
            JwtTokenExpirationDate = GetJwtTokenInfo(x.Tokens, x.Id, "Win32NT").JwtTokenExpirationDate
        }).AsSplitQuery().ToListAsync();
        return result.FirstOrDefault(x => x.JwtToken == token);
    }

    private static List<JwtTokenInfoDto> MapJwtTokenInfoDtos(List<JwtToken> jwtTokens)
    {
        var result = jwtTokens.Where(x => x.AccessTokenExpirationDate > DateTime.Now).Select(x => new JwtTokenInfoDto
        {
            JwtToken = x.AccessToken,
            RefreshToken = x.RefreshToken,
            Device = x.Device,
            RefreshTokenExpirationDate = x.RefreshTokenExpirationDate.ToString(),
            AccountId = x.AccountId,
            JwtTokenExpirationDate = x.AccessTokenExpirationDate.ToString()
        }).ToList();
        return result.Count is 0 ? [] : result;
    }

    private static JwtTokenInfoDto GetJwtTokenInfo(List<JwtToken> jwtTokens, long id, string device)
    {
        var result = jwtTokens.Select(x => new JwtTokenInfoDto
        {
            JwtToken = x.AccessToken,
            RefreshToken = x.RefreshToken,
            Device = "Win32NT",
            RefreshTokenExpirationDate = x.RefreshTokenExpirationDate.ToString(),
            AccountId = id,
            JwtTokenExpirationDate = x.AccessTokenExpirationDate.ToString()
        }).LastOrDefault(x => x.AccountId == id && x.Device == device);
        if (result is null)
            return new JwtTokenInfoDto
            {
                JwtToken = "",
                RefreshToken = "",
                Device = "Win32NT",
                RefreshTokenExpirationDate = DateTime.Now.ToString(),
                JwtTokenExpirationDate = DateTime.Now.ToString(),
                AccountId = id
            };
        if (DateTime.Parse(result.JwtTokenExpirationDate) < DateTime.Now) result.JwtToken = "Don't have invalid token!";
        return result;
    }
}