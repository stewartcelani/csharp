using EFCore.Sqlite.Types;

namespace EFCore.Sqlite.Models;

public class User : Auditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public int Age { get; set; }
    public Sex Sex { get; set; }
    public string? TaxFileNumber { get; set; }

    public IList<UserRole> UserRole { get; set; }
    public virtual IEnumerable<Role> Roles { get; set; }

}