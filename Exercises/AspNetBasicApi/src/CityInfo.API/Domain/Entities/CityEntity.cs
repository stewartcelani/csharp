using CityInfo.API.Domain.Entities.Common;

namespace CityInfo.API.Domain.Entities;

public class CityEntity : AuditableBaseEntity<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    public virtual IList<PointOfInterestEntity> PointsOfInterest { get; set; } = new List<PointOfInterestEntity>();
}