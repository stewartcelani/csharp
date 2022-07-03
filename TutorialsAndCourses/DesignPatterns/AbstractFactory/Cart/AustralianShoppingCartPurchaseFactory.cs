using AbstractFactory.Interfaces;
using AbstractFactory.Services;

namespace AbstractFactory.Cart;

public class AustralianShoppingCartPurchaseFactory : IShoppingCartPurchaseFactory
{
    public IDiscountService CreateDiscountService()
    {
        return new AustralianDiscountService();
    }

    public IShippingCostsService CreateShippingCostsService()
    {
        return new AustralianShippingCostsService();
    }
}