using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace GenericMethodsAndDelegates.Entities;

public static class EntityExtensions
{
    public static TEntity Copy<TEntity>(this TEntity itemToCopy) where TEntity : IEntity
    {
        var jsonString = JsonSerializer.Serialize<TEntity>(itemToCopy);
        return JsonSerializer.Deserialize<TEntity>(jsonString) ?? throw new InvalidOperationException();
    }
}