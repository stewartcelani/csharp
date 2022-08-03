namespace CityInfo.API.Contracts.v1.Requests;

public class CreateCityRequest
{
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
}