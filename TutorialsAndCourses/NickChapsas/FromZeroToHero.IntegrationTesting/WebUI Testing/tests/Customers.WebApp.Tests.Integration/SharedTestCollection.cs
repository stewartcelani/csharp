using Xunit;

namespace Customers.WebApp.Tests.Integration;

[CollectionDefinition("SharedTestCollection")]
public class SharedTestCollection : ICollectionFixture<SharedTestContext>
{
    
}