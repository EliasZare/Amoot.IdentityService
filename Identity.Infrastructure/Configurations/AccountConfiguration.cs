namespace Identity.Infrastructure.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Password).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Name).IsRequired();

        builder
            .HasMany(x => x.Tokens)
            .WithOne(x => x.Account)
            .HasForeignKey(x => x.AccountId).IsRequired(false);
    }
}