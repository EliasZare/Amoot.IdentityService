namespace Identity.Application.Interfaces.Domain;

public interface IJwtTokenService
{
        Task<OperationResponse> GetNewTokenAsync(JwtTokenRequest command);
        Task<OperationResponse> DestroyJwtTokenAsync(JwtTokenRequest command);
        Task<JwtTokenInfoDto> GenerateJwtToken(LoginAccountRequest user);
        Task<AccountInfoDto> GetClaimsFromJwtToken(string token);
        Task<OperationResponse> DestroyAsync(string token);
        Task<OperationResponse> CheckTokenValidation(string token);
    }

