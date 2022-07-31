using System;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Contracts.Requests;

public class UpdateCityRequest
{
    [FromRoute(Name = "cityId")] public Guid Id { get; init; }
    [FromBody] public CreateCityRequest CreateCityRequest { get; set; } = default!;
}