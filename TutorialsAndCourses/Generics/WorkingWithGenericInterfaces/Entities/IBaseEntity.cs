namespace WorkingWithGenericInterfaces.Entities;

public interface IBaseEntity : IEntity<Guid>, IAuditable
{
    
}