namespace CityInfo.API.Contracts.Requests;

public class CreatePointOfInterestRequest
{
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
}