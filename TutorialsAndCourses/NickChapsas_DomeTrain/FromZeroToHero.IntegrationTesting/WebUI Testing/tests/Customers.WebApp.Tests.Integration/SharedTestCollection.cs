using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Customers.WebApp.Tests.Integration;

[ExcludeFromCodeCoverage]
[CollectionDefinition(nameof(SharedTestCollection))]
public class SharedTestCollection : ICollectionFixture<SharedTestContext>
{
    
}