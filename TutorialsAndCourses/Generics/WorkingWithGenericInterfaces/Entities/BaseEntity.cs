namespace WorkingWithGenericInterfaces.Entities;

public class BaseEntity : IEntity<Guid>, IAuditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}