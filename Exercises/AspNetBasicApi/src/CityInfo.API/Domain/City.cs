namespace CityInfo.API.Domain;

public class City
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public List<PointOfInterest>? PointsOfInterest { get; init; }
}