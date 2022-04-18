namespace UptimeR.Application.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity Get(int id);
    TEntity Get(Guid id);
    IEnumerable<TEntity> GetAll();
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    bool Any(IEnumerable<TEntity> entity);
    TEntity Update(TEntity Entity);
    Task<TEntity> GetAsync(int id);
    Task<TEntity> GetAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task<bool> AnyAsync(IEnumerable<TEntity> entity);
}