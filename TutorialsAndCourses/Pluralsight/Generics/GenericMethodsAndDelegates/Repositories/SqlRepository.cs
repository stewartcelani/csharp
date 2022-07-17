using Microsoft.EntityFrameworkCore;
using GenericMethodsAndDelegates.Data;
using GenericMethodsAndDelegates.Entities;

namespace GenericMethodsAndDelegates.Repositories;

public delegate void ItemAdded<in TEntity>(TEntity item);

public class SqlRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    private readonly DataContext _dataContext;
    private readonly DbSet<TEntity> _dbSet;
    private readonly ItemAdded<TEntity>? _itemAddedCallback1;
    private readonly Action<TEntity>? _itemAddedCallback2;

    public SqlRepository(DataContext dataContext, ItemAdded<TEntity>? itemAddedCallback1 = null, Action<TEntity>? itemAddedCallback2 = null)
    {
        _dataContext = dataContext;
        _dbSet = dataContext.Set<TEntity>();
        _itemAddedCallback1 = itemAddedCallback1;
        _itemAddedCallback2 = itemAddedCallback2;
    }

    public event EventHandler<TEntity>? ItemAdded;

    public TEntity GetById(TKey id) => _dbSet.Single(i => EqualityComparer<TKey>.Default.Equals(i.Id, id));

    public IEnumerable<TEntity> GetAll() => _dbSet.ToList();

    public void Add(TEntity item)
    {
        _dbSet.Add(item);
        _itemAddedCallback1?.Invoke(item);
        _itemAddedCallback2?.Invoke(item);
        ItemAdded?.Invoke(this, item);
    }

    public void Save()
    {
        _dataContext.SaveChanges();
    }

    public void Remove(TEntity item) => _dbSet.Remove(item);

}