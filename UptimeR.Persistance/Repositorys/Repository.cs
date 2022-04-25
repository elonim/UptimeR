using Microsoft.EntityFrameworkCore;
using UptimeR.Application.Interfaces;

namespace UptimeR.Persistance.Repositorys;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly DbContext Context;

    public Repository(DbContext context)
    {
        Context = context;
    }

    public TEntity Get(int id) => Context.Set<TEntity>().Find(id)!;
    public TEntity Get(Guid id) => Context.Set<TEntity>().Find(id)!;

    public IEnumerable<TEntity> GetAll() => Context.Set<TEntity>().ToList();

    public void Add(TEntity entity) => Context.Set<TEntity>().Add(entity);

    public void AddRange(IEnumerable<TEntity> entities) => Context.Set<TEntity>().AddRange(entities);

    public void Remove(TEntity entity) => Context.Set<TEntity>().Remove(entity);

    public void RemoveRange(IEnumerable<TEntity> entities) => Context.Set<TEntity>().RemoveRange(entities);

    public bool Any(IEnumerable<TEntity> entity) => Context.Set<TEntity>().Any();

    public TEntity Update(TEntity Entity)
    {
        Context.Update(Entity);
        return Entity;
    }


    public async Task<TEntity> GetAsync(int id) => await Context.Set<TEntity>().FindAsync(id);
    public async Task<TEntity> GetAsync(Guid id) => await Context.Set<TEntity>().FindAsync(id);

    public async Task<IEnumerable<TEntity>> GetAllAsync() => await Context.Set<TEntity>().ToListAsync();

    public async Task AddAsync(TEntity entity) => await Context.Set<TEntity>().AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities) => await Context.Set<TEntity>().AddRangeAsync(entities);
    public async Task<bool> AnyAsync(IEnumerable<TEntity> entity) => await Context.Set<TEntity>().AnyAsync();
}