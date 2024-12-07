namespace Identity.Application.Services;

public class AccountService(
    IAccountRepository _accountRepository,
    IUnitOfWork _unitOfWork) : IAccountService
{
        public async Task<OperationResponse> CreateAsync(CreateAccountRequest command)
        {
            var account = command.Adapt<Account>();
            _accountRepository.CreateAsync(account);
            _unitOfWork.SaveChangesAsync();
            return new OperationResponse(true, "");
        }

        public Task<OperationResponse> UpdateAsync(EditAccountDto command)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResponse> Login(LoginAccountRequest command)
        {
            var account = await _accountRepository.GetAccountByEmailAsync(command.Email);

            if (account == null) return new OperationResponse(false, "حساب کاربری با این اطلاعات وجود ندارد.");

            if (account.Password != command.Password)
                return new OperationResponse(true, "اطلاعات وارد شده صحیح نمس باشد.");

            return new OperationResponse(true, "با موفقیت وارد حساب کاربری خود شدید.");
        }

        public Task<OperationResponse> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResponse> Restore(long id)
        {
            throw new NotImplementedException();
        }

        public Task<EditAccountDto> GetDetailsAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AccountInfoDto>> GetAccountsAsync()
        {
            return await _accountRepository.GetAccountsAsync();
        }

        public async Task<AccountInfoDto> GetAccountByEmailAsync(string email)
        {
            return await _accountRepository.GetByEmail(email);
        }

        public async Task<JwtTokenInfoDto> GetTokensBy(long accountId)
        {
            return await _accountRepository.GetTokensBy(accountId);
        }

        public async Task<AccountInfoDto> GetAccountByToken(string token)
        {
            return await _accountRepository.GetAccountByToken(token);
        }
    }

