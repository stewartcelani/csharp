namespace CityInfo.API.Domain;

public class PointOfInterest
{
    public Guid Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
}