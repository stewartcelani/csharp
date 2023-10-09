namespace GymManagement.Contracts.Subscriptions;

public class SubscriptionResponse
{
    public Guid Id { get; set; }
    public Guid AdminId { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
}