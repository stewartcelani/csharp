using EmployeeManger.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManger.Data;

public class EmployeeManagerDbContext : DbContext
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();

    public EmployeeManagerDbContext(DbContextOptions<EmployeeManagerDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Finance" },
            new Department { Id = 2, Name = "Sales" },
            new Department { Id = 3, Name = "Marketing" },
            new Department { Id = 4, Name = "Human Resources" },
            new Department { Id = 5, Name = "IT" });

        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, FirstName = "Anna", LastName = "Rockstar", DepartmentId = 2 },
            new Employee { Id = 2, FirstName = "Julia", LastName = "Developer", DepartmentId = 5, IsDeveloper = true },
            new Employee { Id = 3, FirstName = "Thomas", LastName = "Huber", DepartmentId = 5, IsDeveloper = true },
            new Employee { Id = 4, FirstName = "Sara", LastName = "Metroid", DepartmentId = 1 },
            new Employee { Id = 5, FirstName = "Ben", LastName = "Rockstar", DepartmentId = 4 },
            new Employee { Id = 6, FirstName = "Alex", LastName = "Rider", DepartmentId = 3, IsDeveloper = true },
            new Employee { Id = 7, FirstName = "Sophie", LastName = "Ramos", DepartmentId = 5 },
            new Employee { Id = 8, FirstName = "Julien", LastName = "Russell", DepartmentId = 2 },
            new Employee { Id = 9, FirstName = "Yvonne", LastName = "Snider", DepartmentId = 4 },
            new Employee { Id = 10, FirstName = "Jasmin", LastName = "Curtis", DepartmentId = 1, IsDeveloper = true });
    }
}