namespace Identity.Application.Mapping;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<CreateAccountDto, Account>
            .NewConfig()
            .Map(des => des.Name, res => res.Name)
            .Map(des => des.Email, res => res.Email)
            .Map(des => des.Mobile, res => res.Mobile)
            .Map(des => des.Password, res => res.Password)
            .Map(des => des.IsDeleted, res => false)
            .Map(des => des.Id, res => 0)
            .Map(des => des.CreationDate, res => DateTime.Now);

        TypeAdapterConfig<CreateJwtTokenDto, JwtToken>
            .NewConfig()
            .Map(des => des.AccessToken, res => res.JwtToken)
            .Map(des => des.AccessTokenExpirationDate, res => res.JwtTokenExpirationDate)
            .Map(des => des.RefreshToken, res => res.RefreshToken)
            .Map(des => des.RefreshTokenExpirationDate, res => res.RefreshTokenExpirationDate)
            .Map(des => des.AccountId, res => res.AccountId)
            .Map(des => des.CreationDate, res => DateTime.Now);

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}

