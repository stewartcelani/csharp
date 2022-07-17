namespace Bridge;

public abstract class Menu
{
    public ICoupon Coupon { get; }
    public abstract int CalculatePrice();

    public Menu(ICoupon coupon)
    {
        Coupon = coupon;
    }
}