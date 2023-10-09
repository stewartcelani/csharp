using GymManagement.Domain.Subscriptions;

namespace GymManagement.Application.Common.Interfaces;

public interface ISubscriptionsRepository
{
    Task<Subscription?> GetByIdAsync(Guid subscriptionId);
    Task AddSubscriptionAsync(Subscription subscription);
}