namespace Observer;

public class TicketStockService : ITicketChangeListener
{
    public void ReceiveTicketChangeNotification(TicketChange ticketChange)
    {
        // update local datastore (omitted in demo)
        Console.WriteLine(
            $"{nameof(TicketStockService)} notified of ticket change: artist {ticketChange.ArtistId}, amount {ticketChange.Amount}");
    }
}