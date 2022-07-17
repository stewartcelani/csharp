namespace WorkingWithGenericInterfaces.Entities;

public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}