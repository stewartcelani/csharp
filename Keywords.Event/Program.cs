using Keywords.Event;

var pubsub = new Pubsub();
Console.WriteLine($"Count: {pubsub.Count}, Members.count: {pubsub.Members.Count}"); // Count: 0, Members: 0
pubsub.OnChange += Methods.HandleOnChange;
pubsub.OnChangeWithParam += Methods.HandleOnChangeWithParam;
pubsub.IncrementCount();
pubsub.IncrementCount();
pubsub.IncrementCount();
pubsub.OnChange -= Methods.HandleOnChange;
pubsub.OnChangeWithParam -= Methods.HandleOnChangeWithParam;
Console.WriteLine($"Count: {pubsub.Count}, Members.count: {pubsub.Members.Count}"); // Count: 3, Members: 3

namespace Keywords.Event
{
    public class Pubsub
    {
        public int Count { get; private set; }
        public List<string> Members { get; set; } = new();
        public event Action? OnChange; // OnChange?.Invoke()
        public event Action<Pubsub, string>? OnChangeWithParam; // OnChangeWithObject?.Invoke(this) - this one will pass reference to self when invoked
        //public event Func<Task> OnChange // Func<Task> is async alternative

        public void IncrementCount()
        {
            Console.WriteLine("IncrementCount");
            Count++;
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
        public static void HandleOnChangeWithParam(Pubsub sender, string name)
        {
            sender.Members.Add(name);
            Console.WriteLine($"HandleOnChangeWithParam, adding member to list");
        }
    }
    
   
}