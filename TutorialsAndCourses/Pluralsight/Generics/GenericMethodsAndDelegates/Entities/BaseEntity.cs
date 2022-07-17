namespace GenericMethodsAndDelegates.Entities;

public abstract class BaseEntity : IEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}