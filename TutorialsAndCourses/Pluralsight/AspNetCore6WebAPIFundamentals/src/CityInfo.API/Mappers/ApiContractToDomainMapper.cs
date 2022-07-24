using CityInfo.API.Contracts.Requests;
using CityInfo.API.Domain;

namespace CityInfo.API.Mappers;

public static class ApiContractToDomainMapper
{
    public static PointOfInterest ToPointOfInterest(this CreatePointOfInterestRequest request)
    {
        return new PointOfInterest
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };
    }
    
    public static PointOfInterest ToPointOfInterest(this UpdatePointOfInterestRequest request)
    {
        return new PointOfInterest
        {
            Id = request.Id,
            Name = request.CreatePointOfInterestRequest.Name,
            Description = request.CreatePointOfInterestRequest.Description
        };
    }
}