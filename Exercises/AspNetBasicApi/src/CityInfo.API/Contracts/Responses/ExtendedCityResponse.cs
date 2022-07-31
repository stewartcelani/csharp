using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Contracts.Responses;

public class ExtendedCityResponse
{
    public Guid Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public int NumberOfPointsOfInterest => PointsOfInterest.Count();

    public IEnumerable<PointOfInterestResponse> PointsOfInterest { get; init; } =
        Enumerable.Empty<PointOfInterestResponse>();
}