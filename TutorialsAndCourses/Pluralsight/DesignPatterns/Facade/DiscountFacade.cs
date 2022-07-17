namespace Facade;

public class DiscountFacade
{
    private readonly OrderService _orderService = new();
    private readonly CustomerDiscountBaseService _customerDiscountBaseService = new();
    private readonly DayOfTheWeekFactorService _dayOfTheWeekFactorService = new();

    public double CalculateDiscountPercentage(int customerId)
    {
        if (!_orderService.HasEnoughOrders(customerId))
        {
            return 0;
        }

        return _customerDiscountBaseService.CalculateDiscountBase(customerId)
               * _dayOfTheWeekFactorService.CalculateDayOfTheWeekFactor();
    }
}