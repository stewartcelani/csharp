using EFCoreWithSqliteConsoleApp.Types;

namespace EFCoreWithSqliteConsoleApp.Entities;

public class User : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public int Age { get; set; }
    public Sex Sex { get; set; }
    public string? TaxFileNumber { get; set; }

    public virtual IList<UserRole> UserRoles { get; set; }
}