using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Contracts.v1.Requests;

public class UpdatePointOfInterestRequest
{
    [FromRoute(Name = "pointOfInterestId")]
    public Guid Id { get; init; }

    [FromBody] public CreatePointOfInterestRequest CreatePointOfInterestRequest { get; set; } = default!;
}