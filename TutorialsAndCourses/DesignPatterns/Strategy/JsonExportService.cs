namespace Strategy;

/// <summary>
/// Concrete Strategy
/// </summary>
public class JsonExportService : IExportService
{
    public void Export(Order order)
    {
        Console.WriteLine($"Exporting {order.Name} to Json.");
    }
}