namespace FactoryMethod.Services.Discount;

public abstract class DiscountFactory
{
    public abstract DiscountService CreateDiscountService();
}