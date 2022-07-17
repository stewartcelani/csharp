using AbstractFactory.Interfaces;

namespace AbstractFactory.Services;

public class BelgiumDiscountService : IDiscountService
{
    public int DiscountPercentage => 20;
}