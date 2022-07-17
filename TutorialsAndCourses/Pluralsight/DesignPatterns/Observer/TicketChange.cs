namespace Observer;

public class TicketChange
{
    public int ArtistId { get; private set; }
    public int Amount { get; private set; }

    public TicketChange(int artistId, int amount)
    {
        ArtistId = artistId;
        Amount = amount;
    }
}