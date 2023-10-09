using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using GymManagement.Infrastructure.Common.Persistence;

namespace GymManagement.Infrastructure.Subscriptions.Persistence;

public class SubscriptionsRepository : ISubscriptionsRepository
{
    private readonly GymManagementDbContext _dbContext;

    public SubscriptionsRepository(GymManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Subscription?> GetByIdAsync(Guid subscriptionId)
    {
        var subscriptionEntity = await _dbContext.Subscriptions.FindAsync(subscriptionId);
        var subscription = subscriptionEntity?.ToSubscription();
        return subscription;
    }

    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        var subscriptionEntity = subscription.ToSubscriptionEntity();
        await _dbContext.Subscriptions.AddAsync(subscriptionEntity);
    }
}