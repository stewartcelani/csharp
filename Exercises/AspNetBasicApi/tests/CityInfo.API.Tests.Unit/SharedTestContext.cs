using System;
using System.Diagnostics.CodeAnalysis;
using Bogus;
using CityInfo.API.Domain;

namespace CityInfo.API.Tests.Unit;

[ExcludeFromCodeCoverage]
public class SharedTestContext
{
    public static readonly Faker<PointOfInterest> PointOfInterestGenerator = new Faker<PointOfInterest>()
        .RuleFor(x => x.Id, Guid.NewGuid())
        .RuleFor(x => x.Name, faker => faker.Lorem.Word())
        .RuleFor(x => x.Description, faker => faker.Lorem.Sentences(2));
    
    public static readonly Faker<City> CityGenerator = new Faker<City>()
        .RuleFor(x => x.Id, Guid.NewGuid())
        .RuleFor(x => x.Name, faker => faker.Address.City())
        .RuleFor(x => x.Description, faker => faker.Lorem.Sentences(2))
        .RuleFor(x => x.PointsOfInterest, PointOfInterestGenerator.Generate(new Random().Next(1, 6)));
}