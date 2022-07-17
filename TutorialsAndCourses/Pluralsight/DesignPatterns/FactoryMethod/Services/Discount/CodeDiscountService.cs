namespace FactoryMethod.Services.Discount;

public class CodeDiscountService : DiscountService
{
    private readonly Guid _code;

    public CodeDiscountService(Guid code)
    {
        _code = code;
    }

    public override int DiscountPercentage
    {
        get
        {
            // In real life this is where you would add a check to see if the code has been used or expired
            return 15;
        }
    }
}