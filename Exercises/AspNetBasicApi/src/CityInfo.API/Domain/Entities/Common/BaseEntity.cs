namespace CityInfo.API.Domain.Entities.Common;

public abstract class BaseEntity<TKey> : BaseEntity
{
    public virtual TKey Id { get; set; } = default!;
}

public abstract class BaseEntity {}