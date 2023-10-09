namespace GymManagement.Domain.Subscriptions;

public class SubscriptionEntity
{
    public Guid Id { get; init; }
    public Guid AdminId { get; init; }
    public SubscriptionType SubscriptionType { get; init; }

    public SubscriptionEntity()
    {
        
    }
    
}