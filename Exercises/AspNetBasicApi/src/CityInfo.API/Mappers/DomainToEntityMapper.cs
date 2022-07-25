using CityInfo.API.Domain;
using CityInfo.API.Domain.Entities;

namespace CityInfo.API.Mappers;

public static class DomainToEntityMapper
{
    public static CityEntity ToCityEntity(this City city)
    {
        return new CityEntity
        {
            Id = city.Id,
            Name = city.Name,
            Description = city.Description,
            PointsOfInterest = city.PointsOfInterest.Select(x => x.ToPointOfInterestEntity(city)).ToList()
        };
    }

    public static PointOfInterestEntity ToPointOfInterestEntity(this PointOfInterest pointOfInterest, City city)
    {
        return new PointOfInterestEntity
        {
            Id = pointOfInterest.Id,
            Name = pointOfInterest.Name,
            Description = pointOfInterest.Description,
            CityId = city.Id
        };
    }
}