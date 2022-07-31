using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace CityInfo.API.Tests.Integration;

[ExcludeFromCodeCoverage]
[CollectionDefinition(nameof(SharedTestCollection))]
public class SharedTestCollection : ICollectionFixture<SharedTestContext>
{
    
}