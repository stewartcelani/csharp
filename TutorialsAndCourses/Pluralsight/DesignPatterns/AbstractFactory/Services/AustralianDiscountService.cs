using AbstractFactory.Interfaces;

namespace AbstractFactory.Services;

public class AustralianDiscountService : IDiscountService
{
    public int DiscountPercentage => 25;
}