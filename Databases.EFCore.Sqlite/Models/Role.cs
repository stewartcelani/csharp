using System.Data.Common;

namespace Databases.EFCore.Sqlite.Models;

public class Role : Auditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }

    public IList<UserRole> UserRole { get; set; }
}