namespace CityInfo.API.Domain;

public class City
{
    public Guid Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public ICollection<PointOfInterest> PointsOfInterest { get; init; } = new List<PointOfInterest>();
}