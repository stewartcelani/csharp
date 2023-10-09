namespace GymManagement.Domain.Subscriptions;

public static class SubscriptionEntityMapper
{
    public static Subscription ToSubscription(this SubscriptionEntity subscriptionEntity)
    {
        return new Subscription(subscriptionEntity.SubscriptionType, subscriptionEntity.AdminId, subscriptionEntity.Id);
    }
}