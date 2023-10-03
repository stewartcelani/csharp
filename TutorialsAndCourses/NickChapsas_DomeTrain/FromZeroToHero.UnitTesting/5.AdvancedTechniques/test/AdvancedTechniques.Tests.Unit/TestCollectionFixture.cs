using Xunit;

namespace AdvancedTechniques.Tests.Unit;

[CollectionDefinition("My awesome collection fixture")]
public class TestCollectionFixture : ICollectionFixture<MyClassFixture>
{
    
}