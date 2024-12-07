namespace Identity.Infrastructure.UnitOfWorks;

public class UnitOfWork(IdentityContext _context) : IUnitOfWork
{
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task CommitAsync()
    {
        throw new NotImplementedException();
    }
}