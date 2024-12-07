namespace Identity.Application.Interfaces.Domain;

public interface IAccountService
{
    Task<OperationResponse> CreateAsync(CreateAccountRequest command);
    Task<OperationResponse> UpdateAsync(EditAccountDto command);
    Task<OperationResponse> Login(LoginAccountRequest command);
    Task<OperationResponse> Delete(long id);
    Task<OperationResponse> Restore(long id);
    Task<EditAccountDto> GetDetailsAsync(long id);
    Task<List<AccountInfoDto>> GetAccountsAsync();
    Task<AccountInfoDto> GetAccountByEmailAsync(string email);
    Task<JwtTokenInfoDto> GetTokensBy(long accountId);
    Task<AccountInfoDto> GetAccountByToken(string token);
}

