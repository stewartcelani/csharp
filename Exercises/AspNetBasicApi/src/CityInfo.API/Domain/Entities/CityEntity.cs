using CityInfo.API.Domain.Entities.Common;

namespace CityInfo.API.Domain.Entities;

public class CityEntity : AuditableBaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public virtual IList<PointOfInterestEntity>? PointsOfInterest { get; set; }
}