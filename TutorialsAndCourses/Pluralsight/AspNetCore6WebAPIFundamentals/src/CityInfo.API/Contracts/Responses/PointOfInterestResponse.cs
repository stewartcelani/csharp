namespace CityInfo.API.Contracts.Responses;

public class PointOfInterestResponse
{
    public Guid Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
}