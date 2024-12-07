namespace Identity.Application.Services;

public class JwtTokenService(
    IConfiguration _configuration,
    IJwtTokenRepository _jwtTokenRepository,
    IUnitOfWork _unitOfWork,
    IAccountService _accountService
) : IJwtTokenService
{
    public async Task<OperationResponse> DestroyJwtTokenAsync(JwtTokenRequest command)
    {
        _jwtTokenRepository.DestroyAsync(command.Token);
        return new OperationResponse(true, "توکن نابود شد.");
    }

    public async Task<JwtTokenInfoDto> GenerateJwtToken(LoginAccountRequest user)
    {
        var account = await _accountService.GetAccountByEmailAsync(user.Email);
        var expires = DateTime.Now.AddMinutes(double.Parse(_configuration["JWT:ExpirationMin"]));

        Claim[] claims =
        {
            new(JwtRegisteredClaimNames.Sub, account.Id.ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var encryptionKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:EncryptionKey"]));

        var encryptingCredentials = new EncryptingCredentials(
            encryptionKey,
            SecurityAlgorithms.Aes128KW, // Key Wrapping Algorithm
            SecurityAlgorithms.Aes128CbcHmacSha256 // Content Encryption Algorithm
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.Now,
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenGenerator = tokenHandler.CreateToken(tokenDescriptor);
        var JwtToken = tokenHandler.WriteToken(tokenGenerator);
        var tokenDto = new JwtTokenInfoDto
        {
            JwtToken = JwtToken,
            RefreshToken = GenerateRefreshToken(),
            RefreshTokenExpirationDate =
                DateTime.Now.AddMinutes(double.Parse(_configuration["RefreshToken:ExpirationMin"])).ToString()
        };
        var jwtToken = tokenDto.Adapt<JwtToken>();
        var deviceName = Environment.MachineName;
        tokenDto.Device = deviceName;
        jwtToken.AccountId = account.Id;
        jwtToken.AccessToken = JwtToken;
        jwtToken.AccessTokenExpirationDate =
            DateTime.Now.AddMinutes(10);
        jwtToken.Device = deviceName;


        var result = await _jwtTokenRepository.CheckDuplicateToken(user.Email, user.DeviceName);
        if (result)
            await _jwtTokenRepository.UpdateAsync(jwtToken);
        else
            await _jwtTokenRepository.CreateAsync(jwtToken);

        await _unitOfWork.SaveChangesAsync();
        return tokenDto;
    }

    public async Task<AccountInfoDto> GetClaimsFromJwtToken(string token)
    {
        var encryptionKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:EncryptionKey"]));
        var account = new AccountInfoDto();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = _configuration["JWT:Audience"],
            ValidateIssuer = true,
            ValidIssuer = _configuration["JWT:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"])),
            ValidateLifetime = false,
            TokenDecryptionKey = encryptionKey
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        JwtBearerEvents jwtEvents = new JwtBearerEvents();
        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Innnnnnnnnnnnnnnnnnnvalid Token!");
        account.Mobile = claims.FindFirst(ClaimTypes.MobilePhone).ToString();

        return account;
    }

    public async Task<OperationResponse> DestroyAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return new OperationResponse(false, "عملیات با شکست مواجه شد! توکن معتبر نیست.");
        await _jwtTokenRepository.DestroyAsync(token);
        await _unitOfWork.SaveChangesAsync();
        return new OperationResponse(true, "عملیات نابود سازی با موفقیت انجام شد.");
    }

    public async Task<OperationResponse> CheckTokenValidation(string token)
    {
        var result = await _jwtTokenRepository.CheckTokenValidation(token);
        return result
            ? new OperationResponse(result, " عملیات با موفقیت انجام شد.")
            : new OperationResponse(result, "عملیات با شکست مواجه شد.");
    }

    public async Task<OperationResponse> GetNewTokenAsync(JwtTokenRequest command)
    {
        var account = await _accountService.GetAccountByToken(command.Token);

        if (account == null)
            return new OperationResponse(false, "رکوردی با این اطلاعات یافت نشد.");

        var token = await _accountService.GetTokensBy(account.Id);

        if (token.RefreshToken != command.RefreshToken &&
            Convert.ToDateTime(token.RefreshTokenExpirationDate) <= DateTime.Now)
            return new OperationResponse(false, "رفرش توکن معتبر نبست!");


        var expires = DateTime.Now.AddMinutes(double.Parse(_configuration["JWT:ExpirationMin"]));

        Claim[] claims =
        {
            new(JwtRegisteredClaimNames.Sub, account.Id.ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var encryptionKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:EncryptionKey"]));

        var encryptingCredentials = new EncryptingCredentials(
            encryptionKey,
            SecurityAlgorithms.Aes128KW, // Key Wrapping Algorithm
            SecurityAlgorithms.Aes128CbcHmacSha256 // Content Encryption Algorithm
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.Now,
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenGenerator = tokenHandler.CreateToken(tokenDescriptor);
        var JwtToken = tokenHandler.WriteToken(tokenGenerator);
        var tokenDto = new JwtTokenInfoDto
        {
            JwtToken = JwtToken,
            RefreshToken = GenerateRefreshToken(),
            RefreshTokenExpirationDate =
                DateTime.Now.AddMinutes(double.Parse(_configuration["RefreshToken:ExpirationMin"])).ToString()
        };
        var jwtToken = tokenDto.Adapt<JwtToken>();
        var os = Environment.OSVersion;
        tokenDto.Device = os.Platform.ToString();
        jwtToken.AccountId = account.Id;
        jwtToken.AccessToken = JwtToken;
        jwtToken.AccessTokenExpirationDate =
            DateTime.Now.AddMinutes(10);
        jwtToken.Device = os.Platform.ToString();
        await _jwtTokenRepository.CreateAsync(jwtToken);
        await _jwtTokenRepository.DestroyAsync(command.Token);
        await _unitOfWork.SaveChangesAsync();
        return new OperationResponse(true, "توکن جدید با موفقیت ساحتته شد .", tokenDto.JwtToken);
    }

    private string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}