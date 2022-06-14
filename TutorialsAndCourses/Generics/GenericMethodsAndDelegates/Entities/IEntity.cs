namespace GenericMethodsAndDelegates.Entities;

public interface IEntity : IAuditable 
{}

/*
 * Using generic IEntity<TKey> was not part of the course
 * There might be situations where you want a class that just implements IEntity and has no "Id" (join tables etc)
 */
public interface IEntity<TKey> : IEntity
{
    TKey Id { get; set; }
}

