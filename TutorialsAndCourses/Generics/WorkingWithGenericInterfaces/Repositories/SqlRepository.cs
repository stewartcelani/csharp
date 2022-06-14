using Microsoft.EntityFrameworkCore;
using WorkingWithGenericInterfaces.Data;
using WorkingWithGenericInterfaces.Entities;

namespace WorkingWithGenericInterfaces.Repositories;

public class SqlRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    private readonly DataContext _dataContext;
    private readonly DbSet<TEntity> _dbSet;

    public SqlRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
        _dbSet = dataContext.Set<TEntity>();
    } 

    public TEntity GetById(TKey id) => _dbSet.Single(i => EqualityComparer<TKey>.Default.Equals(i.Id, id));

    public IEnumerable<TEntity> GetAll() => _dbSet.ToList();

    public void Add(TEntity item)
    {
        _dbSet.Add(item);
    }

    public void Save()
    {
        _dataContext.SaveChanges();
    }

    public void Remove(TEntity item) => _dbSet.Remove(item);
}