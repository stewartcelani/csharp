using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Contracts.Requests;
using CityInfo.API.Domain;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using Xunit;

namespace CityInfo.API.Tests.Integration;

[ExcludeFromCodeCoverage]
public class SharedTestContext : IAsyncLifetime
{
      public readonly Faker<CreateCityRequest> CityRequestGenerator = new Faker<CreateCityRequest>()
        .RuleFor(x => x.Name, faker => faker.Address.City())
        .RuleFor(x => x.Description, faker => faker.Lorem.Sentences(new Random().Next(2, 4)));
    
    public readonly Faker<City> CityGenerator = new Faker<City>()
        .RuleFor(x => x.Id, faker => faker.Random.Guid())
        .RuleFor(x => x.Name, faker => faker.Address.City())
        .RuleFor(x => x.Description, faker => faker.Lorem.Sentences(2))
        .RuleFor(x => x.PointsOfInterest, _ => PointOfInterestGenerator.Generate(new Random().Next(1, 10)));
    
    private static readonly Faker<PointOfInterest> PointOfInterestGenerator = new Faker<PointOfInterest>()
        .RuleFor(x => x.Id, faker => faker.Random.Guid())
        .RuleFor(x => x.Name, faker => faker.Lorem.Word())
        .RuleFor(x => x.Description, faker => faker.Lorem.Sentences(2));

    public const string AppUrl = "https://localhost:7780";
    
    private static readonly string DockerComposeFile =
        Path.Combine(Directory.GetCurrentDirectory(), (TemplateString)"../../../docker-compose.integration.yml");

    public readonly HttpClient HttpClient;

    public SharedTestContext()
    {
        ServicePointManager.ServerCertificateValidationCallback +=
            (sender, certificate, chain, errors) => true;
        HttpClient = new HttpClient();
        HttpClient.BaseAddress = new Uri($"{AppUrl}/");
    }
    
    private readonly ICompositeService _dockerService = new Builder()
        .UseContainer()
        .UseCompose()
        .FromFile(DockerComposeFile)
        .RemoveOrphans()
        .ForceRecreate() // This will ensure the database is recreated every time tests are launched
        .WaitForHttp("cityinfo-test-api", $"{AppUrl}/api/cities")
        .Build();

    public async Task InitializeAsync()
    {
        _dockerService.Start();
        await Task.Delay(2000);
    }

    public new Task DisposeAsync()
    {
        HttpClient.Dispose();
        foreach (var dockerServiceContainer in _dockerService.Containers)
        {
            dockerServiceContainer.Image.Remove();
        }
        _dockerService.Dispose();
        return Task.CompletedTask;
    }
}