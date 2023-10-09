using Ardalis.SmartEnum;

namespace GymManagement.Domain.Subscriptions;

public class SubscriptionType : SmartEnum<SubscriptionType>
{
    public static readonly SubscriptionType Free = new(nameof(Free), 0);
    public static readonly SubscriptionType Starter = new(nameof(Starter), 1);
    public static readonly SubscriptionType Pro = new(nameof(Pro), 2);
        
    public SubscriptionType(string name, int value) : base(name, value)
    {
    }
    
    public static implicit operator SubscriptionType(string value) => FromName(value);
    
    public static implicit operator Contracts.Subscriptions.SubscriptionType(SubscriptionType smartEnum)
    {
        return smartEnum.Name switch
        {
            nameof(Free) => Contracts.Subscriptions.SubscriptionType.Free,
            nameof(Starter) => Contracts.Subscriptions.SubscriptionType.Starter,
            nameof(Pro) => Contracts.Subscriptions.SubscriptionType.Pro,
            _ => throw new ArgumentException($"Invalid SubscriptionType: {smartEnum.Name}")
        };
    }
}
