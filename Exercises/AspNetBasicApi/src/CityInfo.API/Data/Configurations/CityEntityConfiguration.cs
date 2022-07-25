using CityInfo.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityInfo.API.Data.Configurations;

public class CityEntityConfiguration : IEntityTypeConfiguration<CityEntity>
{
    public void Configure(EntityTypeBuilder<CityEntity> builder)
    {
        /* // Explicit one-to-many, not required for deletes on CityEntity to cascade to PointOfInterestEntity
         builder
            .HasMany(x => x.PointsOfInterest)
            .WithOne(x => x.City)
            .IsRequired();
            */
        
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