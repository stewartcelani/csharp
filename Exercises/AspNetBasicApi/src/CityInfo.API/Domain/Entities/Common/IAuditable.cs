namespace CityInfo.API.Domain.Entities.Common;

public interface IAuditable
{
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset? DateUpdated { get; set; }
}