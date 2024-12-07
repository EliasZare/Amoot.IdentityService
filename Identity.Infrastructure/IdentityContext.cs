namespace Identity.Infrastructure;

public class IdentityContext(DbContextOptions<IdentityContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<JwtToken> JwtTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(AccountConfiguration).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        base.OnModelCreating(modelBuilder);
    }
}