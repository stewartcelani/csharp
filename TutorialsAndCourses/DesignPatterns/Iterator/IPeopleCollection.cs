namespace Iterator;

/// <summary>
/// Aggregate
/// </summary>
public interface IPeopleCollection
{
    IPeopleIterator CreateIterator();
}