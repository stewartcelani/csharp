namespace CityInfo.API.Contracts.v1.Responses;

public class ExtendedCityResponse
{
    public Guid Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public int NumberOfPointsOfInterest => PointsOfInterest.Count();

    public IEnumerable<PointOfInterestResponse> PointsOfInterest { get; init; } =
        Enumerable.Empty<PointOfInterestResponse>();
}