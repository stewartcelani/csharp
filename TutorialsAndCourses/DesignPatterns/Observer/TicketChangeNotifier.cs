namespace Observer;

/// <summary>
/// Subject
/// </summary>
public abstract class TicketChangeNotifier
{
    private List<ITicketChangeListener> _observers = new();

    public void AddObserver(ITicketChangeListener observer) => _observers.Add(observer);
    
    public void RemoveObserver(ITicketChangeListener observer) => _observers.Remove(observer);

    public void Notifiy(TicketChange ticketChange)
    {
        foreach (var ticketChangeListener in _observers)
        {
            ticketChangeListener.ReceiveTicketChangeNotification(ticketChange);
        }
    }
}