using EFCoreOptimizationBenchmarks.Types;

namespace EFCoreOptimizationBenchmarks.Entities;

public class User : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public int Age { get; set; }
    public Sex Sex { get; set; }
    public string? TaxFileNumber { get; set; }
    public string? DriversLicenseNumber { get; set; }
    public string Title { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Postcode { get; set; }
    public string Country { get; set; }
    public string HomePhone { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }

    public virtual IList<UserRole> UserRoles { get; set; }
}