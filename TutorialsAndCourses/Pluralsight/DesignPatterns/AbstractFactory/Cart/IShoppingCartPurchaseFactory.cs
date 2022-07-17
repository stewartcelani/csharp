using AbstractFactory.Interfaces;

namespace AbstractFactory.Services;

public interface IShoppingCartPurchaseFactory
{
    IDiscountService CreateDiscountService();
    IShippingCostsService CreateShippingCostsService();
}