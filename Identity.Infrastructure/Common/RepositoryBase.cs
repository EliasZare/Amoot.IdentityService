namespace Identity.Infrastructure.Common;

public class RepositoryBase<TKey, T>(IdentityContext _context) : IRepositoryBase<TKey, T> where T : BaseEntity
{
    public async Task CreateAsync(T entity)
    {
        await _context.AddAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Update(entity);
    }

    public async Task<T?> FindAsync(TKey id)
    {
        return await _context.FindAsync<T>(id);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Any(expression);
    }
}