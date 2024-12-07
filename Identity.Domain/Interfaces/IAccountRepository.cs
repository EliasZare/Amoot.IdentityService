namespace Identity.Domain.Interfaces;

public interface IAccountRepository : IRepositoryBase<long, Account>
{
        Task<AccountInfoDto> GetByEmail(string email);
        Task<Account> GetAccountByEmailAsync(string email);
        Task<List<AccountInfoDto>> GetAccountsAsync();
        Task<JwtTokenInfoDto> GetTokensBy(long accountId);
        Task<AccountInfoDto> GetAccountByToken(string token);
    }

