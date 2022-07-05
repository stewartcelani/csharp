namespace Strategy;

/// <summary>
/// Concrete Strategy
/// </summary>
public class CsvExportService : IExportService
{
    public void Export(Order order)
    {
        Console.WriteLine($"Exporting {order.Name} to Csv.");
    }
}