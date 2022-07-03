namespace AbstractFactory.Interfaces;

class AustralianShippingCostsService : IShippingCostsService
{
    public decimal ShippingCosts => 5m;
}