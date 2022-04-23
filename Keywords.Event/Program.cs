using Keywords.Event;

var pubsub = new Pubsub();
var reportServer = new ReportServer(); // Simulate another project/service
Console.WriteLine($"Count: {pubsub.UsersOnline}, Members.count: {pubsub.Users.Count}"); // Count: 0, Members: 0
pubsub.OnChange += Methods.HandleOnChange;
pubsub.OnChangeWithParam += Methods.HandleOnChangeWithParam;
/*
 * reportServer.PrecalculateStatisticsForUser is an example of a statistics dashboard with computationally expensive
 * calculations that, before events, you would have to have some sort of timer on a schedule checking for new login
 * events and running then. You would still check you weren't generating too often if user was disconnecting/reconnecting
 * often in this example.
 */
pubsub.OnChangeWithParam += reportServer.PrecalculateStatisticsForUser; // Can subscribe more than 1 method to same event
pubsub.HandleUserConnect(); // The method that invokes the event, notifying all subscribers
pubsub.HandleUserConnect();
pubsub.HandleUserConnect();
pubsub.OnChange -= Methods.HandleOnChange;
pubsub.OnChangeWithParam -= Methods.HandleOnChangeWithParam;
pubsub.OnChangeWithParam -= reportServer.PrecalculateStatisticsForUser;
Console.WriteLine($"Count: {pubsub.UsersOnline}, Members.count: {pubsub.Users.Count}"); // Count: 3, Members: 3


namespace Keywords.Event
{
    public class Pubsub
    {
        public int UsersOnline { get; private set; }
        public List<string> Users { get; set; } = new();
        public event Action? OnChange; // OnChange?.Invoke()
        public event Action<Pubsub, string>? OnChangeWithParam; // OnChangeWithObject?.Invoke(this) - this one will pass reference to self when invoked
        //public event Func<Task> OnChange // Func<Task> is async alternative

        public void HandleUserConnect()
        {
            Console.WriteLine("HandleUserConnect");
            UsersOnline++;
            OnChange?.Invoke();
            OnChangeWithParam?.Invoke(this, "Chris"); // OnChange?. makes sure event is only invoked if there are subscribers
        }

    }

    public static class Methods
    {
        public static void HandleOnChange()
        {
            Console.WriteLine("HandleOnChange");
        }
        public static void HandleOnChangeWithParam(Pubsub sender, string user)
        {
            sender.Users.Add(user);
            Console.WriteLine($"HandleOnChangeWithParam, adding user to list");
        }
    }

    public class ReportServer
    {
        public void PrecalculateStatisticsForUser(Pubsub sender, string user)
        {
            Console.WriteLine($"Doing expensive calculations for user: {user}");
        }
    }
    
   
}