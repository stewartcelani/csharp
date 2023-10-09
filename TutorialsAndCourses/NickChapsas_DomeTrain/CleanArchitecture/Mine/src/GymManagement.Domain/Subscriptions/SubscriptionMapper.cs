using GymManagement.Contracts.Subscriptions;

namespace GymManagement.Domain.Subscriptions;

public static class SubscriptionMapper
{
    public static SubscriptionEntity ToSubscriptionEntity(this Subscription subscription)
    {
        return new SubscriptionEntity
        {
            Id = subscription.Id,
            AdminId = subscription.AdminId,
            SubscriptionType = subscription.SubscriptionType
        };
    }

    public static SubscriptionResponse ToSubscriptionResponse(this Subscription subscription)
    {
        return new SubscriptionResponse
        {
            Id = subscription.Id,
            AdminId = subscription.AdminId,
            SubscriptionType = subscription.SubscriptionType
        };
    }
}