namespace GymManagement.Contracts.Subscriptions;

public record CreateSubscriptionRequest(string SubscriptionType, Guid AdminId);