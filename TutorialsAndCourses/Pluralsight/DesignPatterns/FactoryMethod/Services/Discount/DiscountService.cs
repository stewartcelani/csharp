namespace FactoryMethod.Services.Discount;

public abstract class DiscountService
{
    public abstract int DiscountPercentage { get; }

    public override string ToString() => GetType().Name;
}