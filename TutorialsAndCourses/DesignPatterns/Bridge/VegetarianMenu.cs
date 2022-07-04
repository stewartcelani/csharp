namespace Bridge;

public class VegetarianMenu : Menu
{
    public VegetarianMenu(ICoupon coupon) : base(coupon)
    {
    }

    public override int CalculatePrice()
    {
        return 20 - Coupon.CouponValue;
    }
}