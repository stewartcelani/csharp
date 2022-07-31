using System;
using CityInfo.API.Domain.Entities;
using CityInfo.API.Repositories.Common;

namespace CityInfo.API.Repositories;

public interface IPointOfInterestRepository : IRepository<PointOfInterestEntity, Guid>
{
}