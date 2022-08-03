namespace CityInfo.API.Contracts.v1.Responses;

public class PointOfInterestResponse
{
    public Guid Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
}