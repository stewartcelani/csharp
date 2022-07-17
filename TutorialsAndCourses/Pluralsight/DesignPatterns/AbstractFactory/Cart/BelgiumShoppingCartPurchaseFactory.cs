using AbstractFactory.Interfaces;
using AbstractFactory.Services;

namespace AbstractFactory.Cart;

public class BelgiumShoppingCartPurchaseFactory : IShoppingCartPurchaseFactory
{
    public IDiscountService CreateDiscountService()
    {
        return new BelgiumDiscountService();
    }

    public IShippingCostsService CreateShippingCostsService()
    {
        return new BelgiumShippingCostsService();
    }
}