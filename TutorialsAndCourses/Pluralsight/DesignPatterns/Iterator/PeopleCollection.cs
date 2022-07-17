namespace Iterator;

/// <summary>
/// ConcreteAggregate
/// </summary>
public class PeopleCollection : List<Person>, IPeopleCollection
{
    public IPeopleIterator CreateIterator()
    {
        return new PeopleIterator(this);
    }
}