using CityInfo.API.Domain;
using CityInfo.API.Domain.Entities;

namespace CityInfo.API.Mappers;

public static class EntityToDomainMapper
{
    public static City ToCity(this CityEntity cityEntity)
    {
        return new City
        {
            Id = cityEntity.Id,
            Name = cityEntity.Name,
            Description = cityEntity.Description,
            PointsOfInterest = cityEntity.PointsOfInterest?.Select(x => x.ToPointOfInterest()).ToList()
        };
    }

    public static PointOfInterest ToPointOfInterest(this PointOfInterestEntity pointOfInterestEntity)
    {
        return new PointOfInterest
        {
            Id = pointOfInterestEntity.Id,
            Name = pointOfInterestEntity.Name,
            Description = pointOfInterestEntity.Description
        };
    }
}