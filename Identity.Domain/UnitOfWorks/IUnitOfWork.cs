namespace Identity.Domain.UnitOfWorks;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
    Task CommitAsync();
}

