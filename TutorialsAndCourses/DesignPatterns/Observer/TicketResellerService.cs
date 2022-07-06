namespace Observer;

public class TicketResellerService : ITicketChangeListener
{
    public void ReceiveTicketChangeNotification(TicketChange ticketChange)
    {
        // update local datastore (omitted in demo)
        Console.WriteLine(
            $"{nameof(TicketResellerService)} notified of ticket change: artist {ticketChange.ArtistId}, amount {ticketChange.Amount}");
    }
}