namespace CityInfo.API.Contracts.v1.Requests;

public class CreatePointOfInterestRequest
{
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
}