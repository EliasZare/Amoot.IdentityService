namespace Identity.Domain.Interfaces.Common;

public interface IRepositoryBase<TKey, T>
{
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task<T?> FindAsync(TKey id);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
}