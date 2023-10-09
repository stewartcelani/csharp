namespace GymManagement.Domain.Subscriptions;

public class Subscription
{
    public Guid Id { get; }
    public Guid AdminId { get; }
    public SubscriptionType SubscriptionType { get;}

    public Subscription(SubscriptionType subscriptionType, Guid adminId, Guid? id = null)
    {
        SubscriptionType = subscriptionType;
        AdminId = adminId;
        Id = id ?? Guid.NewGuid();
    }
    
}