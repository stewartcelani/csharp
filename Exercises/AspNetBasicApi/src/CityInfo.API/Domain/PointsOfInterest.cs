using System;

namespace CityInfo.API.Domain;

public class PointOfInterest
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
}