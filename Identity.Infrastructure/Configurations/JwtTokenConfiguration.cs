namespace Identity.Infrastructure.Configurations;

public class JwtTokenConfiguration : IEntityTypeConfiguration<JwtToken>
{
    public void Configure(EntityTypeBuilder<JwtToken> builder)
    {
        builder.ToTable("JwtTokens");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AccessToken).IsRequired();

        builder
            .HasOne(x => x.Account)
            .WithMany(x => x.Tokens)
            .HasForeignKey(x => x.AccountId).IsRequired(false);
    }
}