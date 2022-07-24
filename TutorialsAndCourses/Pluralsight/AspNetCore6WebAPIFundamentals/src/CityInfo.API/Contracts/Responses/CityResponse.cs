namespace CityInfo.API.Contracts.Responses;

public class CityResponse
{
    public Guid Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public int NumberOfPointsOfInterest { get; init; } = default!;
    public IEnumerable<PointOfInterestResponse> PointsOfInterest { get; init; } = new List<PointOfInterestResponse>();
}