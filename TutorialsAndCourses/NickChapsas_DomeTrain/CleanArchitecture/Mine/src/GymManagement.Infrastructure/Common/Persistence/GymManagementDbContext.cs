using System.Data.Common;
using System.Reflection;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common.Persistence;

public class GymManagementDbContext : DbContext, IUnitOfWork
{
    public DbSet<SubscriptionEntity> Subscriptions { get; set; } = null!;
    
    public GymManagementDbContext(DbContextOptions options): base(options) 
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task CommitChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}