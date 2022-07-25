namespace CityInfo.API.Contracts.Requests;

public class CreateCityRequest
{
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
}