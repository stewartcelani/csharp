namespace Observer;

/// <summary>
/// Observer
/// </summary>
public interface ITicketChangeListener
{
    void ReceiveTicketChangeNotification(TicketChange ticketChange);
}