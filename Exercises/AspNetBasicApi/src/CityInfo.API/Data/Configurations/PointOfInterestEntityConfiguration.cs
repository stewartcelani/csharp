using CityInfo.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityInfo.API.Data.Configurations;

public class PointOfInterestEntityConfiguration : IEntityTypeConfiguration<PointOfInterestEntity>
{
    public void Configure(EntityTypeBuilder<PointOfInterestEntity> builder)
    {
        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Description)
            .IsRequired();

        builder.Property(x => x.DateUpdated)
            .HasDefaultValue(null);

        builder.Property(x => x.DateCreated)
            .HasDefaultValueSql("now()");
    }
}