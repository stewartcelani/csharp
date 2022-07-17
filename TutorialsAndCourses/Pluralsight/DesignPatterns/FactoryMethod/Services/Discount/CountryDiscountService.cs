namespace FactoryMethod.Services.Discount;

public class CountryDiscountService : DiscountService
{
    private readonly string _countryIdentifier;

    public CountryDiscountService(string countryIdentifier)
    {
        _countryIdentifier = countryIdentifier;
    }

    public override int DiscountPercentage
    {
        get
        {
            return _countryIdentifier switch
            {
                "AU" => 20,
                _ => 10
            };
        }
    }
}