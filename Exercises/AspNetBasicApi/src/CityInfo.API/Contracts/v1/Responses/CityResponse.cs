namespace CityInfo.API.Contracts.v1.Responses;

public class CityResponse
{
    public Guid Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
}