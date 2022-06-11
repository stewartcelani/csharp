namespace EFCoreWithSqliteConsoleApp.Entities;

public class Role : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    
    public virtual IList<UserRole> UserRoles { get; set; }
    
}