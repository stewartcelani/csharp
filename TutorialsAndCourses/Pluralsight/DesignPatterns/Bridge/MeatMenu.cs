namespace Bridge;

public class MeatMenu : Menu
{
    public MeatMenu(ICoupon coupon) : base(coupon)
    {
    }

    public override int CalculatePrice()
    {
        return 30 - Coupon.CouponValue;
    }
}