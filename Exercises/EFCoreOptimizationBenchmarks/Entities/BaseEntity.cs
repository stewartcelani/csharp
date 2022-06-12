namespace EFCoreOptimizationBenchmarks.Entities;

public abstract class BaseEntity
{
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset? DateUpdated { get; set; }
    public DateTimeOffset? DateArchived { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
}