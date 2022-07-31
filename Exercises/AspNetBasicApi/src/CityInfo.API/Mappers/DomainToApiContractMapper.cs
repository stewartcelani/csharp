using System.Linq;
using CityInfo.API.Contracts.Requests;
using CityInfo.API.Contracts.Responses;
using CityInfo.API.Domain;

namespace CityInfo.API.Mappers;

public static class DomainToApiContractMapper
{
    public static CityResponse ToCityResponse(this City city)
    {
        return new CityResponse
        {
            Id = city.Id,
            Name = city.Name,
            Description = city.Description
        };
    }

    public static ExtendedCityResponse ToExtendedCityResponse(this City city)
    {
        return new ExtendedCityResponse
        {
            Id = city.Id,
            Name = city.Name,
            Description = city.Description,
            PointsOfInterest = city.PointsOfInterest.Select(x => x.ToPointOfInterestResponse())
        };
    }

    public static PointOfInterestResponse ToPointOfInterestResponse(this PointOfInterest pointOfInterest)
    {
        return new PointOfInterestResponse
        {
            Id = pointOfInterest.Id,
            Name = pointOfInterest.Name,
            Description = pointOfInterest.Description
        };
    }

    public static CreatePointOfInterestRequest ToCreatePointOfInterestRequest(this PointOfInterest pointOfInterest)
    {
        return new CreatePointOfInterestRequest
        {
            Name = pointOfInterest.Name,
            Description = pointOfInterest.Description
        };
    }

    public static UpdatePointOfInterestRequest ToUpdatePointOfInterestRequest(this PointOfInterest pointOfInterest)
    {
        return new UpdatePointOfInterestRequest
        {
            Id = pointOfInterest.Id,
            CreatePointOfInterestRequest = pointOfInterest.ToCreatePointOfInterestRequest()
        };
    }
}