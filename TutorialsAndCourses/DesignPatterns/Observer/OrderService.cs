namespace Observer;

/// <summary>
/// ConcreteSubject
/// </summary>
public class OrderService : TicketChangeNotifier
{
    public void CompleteTicketSale(int artistId, int amount)
    {
        // change local datastore. Datastore omitted in demo implementation.
        Console.WriteLine($"{nameof(OrderService)} is changing its state.");
        // notify observers
        Console.WriteLine($"{nameof(OrderService)} is notifying observers...");
        Notifiy(new TicketChange(artistId, amount));
    }
}